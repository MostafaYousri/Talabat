
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Core.Specifications.Order_Specs;

namespace Talabat.Services
{
    public class OrderService : IOrderService
    {
        private readonly IPaymentService _paymentService;
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        ///private readonly IGenericRepository<Product> productRepo;
        ///private readonly IGenericRepository<DeliveryMethod> _deliveryMethodRepo;
        ///private readonly IGenericRepository<Order> _orderRepo;

        public OrderService(
            IPaymentService paymentService,
            IBasketRepository basketRepository,
            IUnitOfWork unitOfWork
            ///IGenericRepository<Product> productRepo,
            ///IGenericRepository<DeliveryMethod> deliveryMethodRepo,
            ///IGenericRepository<Order> OrderRepo
            )
        {
            _paymentService = paymentService;
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            ///this.productRepo = productRepo;
            ///_deliveryMethodRepo = deliveryMethodRepo;
            ///_orderRepo = OrderRepo;
        }


        public async Task<Order?> CreateOrderAsync(string BuyerEmail, string BasketId, int DeliveryMethodId, Address Shipping)
        {
            // 1 - Get Basket From Basket Repo
            var basket = await _basketRepository.GetBasketAsync(BasketId);

            // 2 - Get Selected Items as Basket From Product Repo
            var orderItems = new List<OrderItem>();

            if(basket?.Items?.Count >0)
            {
                var productRepository = _unitOfWork.Repository<Product>();
                foreach (var item in basket.Items)
                {
                    var product = await productRepository.GetByIdAsync(item.Id);

                    var productItemOrdered = new ProductItemOrdered(item.Id, product.Name, product.PictureUrl);

                    var orderItem = new OrderItem(productItemOrdered, product.Price, item.Quantity);

                    orderItems.Add(orderItem);
                }
            }
            // 3 - Calculate SubTotal
            var subTotal = orderItems.Sum(Orderitem => Orderitem.Quantity * Orderitem.Price);

            // 4 - Get Delivery Method from Delivery Method Repo

            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(DeliveryMethodId);

            var orderRepo = _unitOfWork.Repository<Order>();

            var orderSpecs = new OrderWithPaymentIntentSpecifications(basket.PaymentIntentId);

            var existingOrder = await orderRepo.GetEntityWithSpecAsync(orderSpecs);
            
            if(existingOrder != null)
            {
                orderRepo.Delete(existingOrder);
                await _paymentService.CreateOrUpdatePaymentIntent(basket.Id);
            }
            // 5 - Create Order
            var order = new Order(BuyerEmail, Shipping, deliveryMethod, orderItems, subTotal,basket.PaymentIntentId);
            await orderRepo.AddAsync(order);

            // 6 - save to Database
            var Result= await _unitOfWork.CompleteAsync();
            if (Result <= 0) return null;
            return order;




        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string BuyerEmail)
        {
            var OrdersRepo =  _unitOfWork.Repository<Order>();
            var spec = new OrderSpecifications(BuyerEmail);

            var orders = await OrdersRepo.GetAllWithSpecAsync(spec);
            return orders;
        }
        public async Task<Order?> GetOrderByIdForUserAsync(int OrderId, string BuyerEmail)
            => await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(new OrderSpecifications(OrderId, BuyerEmail));

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        => await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
    }
}
