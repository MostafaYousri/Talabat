using AutoMapper;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Entities.Order_Aggregate;
using TalabatAPIs.DTOs;

using UserAddress = Talabat.Core.Entities.Identity.Address;
using OrderAddress = Talabat.Core.Entities.Order_Aggregate.Address;
namespace TalabatAPIs.Helpers
{
    public class MappingProfiles :Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                      .ForMember(d => d.ProductType, O => O.MapFrom(S => S.ProductType.Name))
                      .ForMember(d => d.ProductBrand, O => O.MapFrom(S => S.ProductBrand.Name))
                      .ForMember(d => d.PictureUrl , O => O.MapFrom<ProductPictureUrlResolver>()) ;

            CreateMap<AddressDto, OrderAddress>();

            CreateMap<UserAddress, AddressDto>().ReverseMap();

            CreateMap<Order, OrderToReturnDto>().
                ForMember(d => d.DeliveryMethod, O =>O.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.DeliveryMethodCost, O=> O.MapFrom(s => s.DeliveryMethod.Cost));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductName, O =>O.MapFrom(s => s.Product.ProductName))
                .ForMember(d => d.ProductId, O =>O.MapFrom(s => s.Product.ProductId))
                .ForMember(d => d.PictureUrl, O =>O.MapFrom(s => s.Product.PictureUrl))
                .ForMember(d => d.PictureUrl,O=>O.MapFrom<OrderItemPictureUrlResolver>());
            CreateMap<CustomerBasketDto, CustomerBasket>();
        }
    }
}
