using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Services;
using Talabat.Core.Specifications;

namespace Talabat.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IReadOnlyList<Product>> GetProductsAsync(ProductSpecParams Params)
        {
            var Spec = new ProductWithBrandAndTypeSpecifications(Params);
            var Products = await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(Spec);

            return Products;
        }
        public async Task<Product?> GetProductAsync(int ProductId)
        {
            var Spec = new ProductWithBrandAndTypeSpecifications(ProductId);
            var product = await _unitOfWork.Repository<Product>().GetEntityWithSpecAsync(Spec);
            return product;
        }    
        public async Task<int> GetCountAsync(ProductSpecParams Params)
        {
            var CountSpec = new ProductWithFiltrationForCountAsync(Params);
            var Count = await _unitOfWork.Repository<Product>().GetCountWithSpecAsync(CountSpec);

            return Count;
        }
        public async Task<IReadOnlyList<ProductBrand>> GetBrandsAsync()
         => await _unitOfWork.Repository<ProductBrand>().GetAllAsync();


        public async Task<IReadOnlyList<ProductType>> GetTypesAsync()
          => await _unitOfWork.Repository<ProductType>().GetAllAsync();


    }
}
