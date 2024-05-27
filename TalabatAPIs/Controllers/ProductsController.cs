using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Core.Specifications;
using TalabatAPIs.DTOs;
using TalabatAPIs.Errors;
using TalabatAPIs.Helpers;

namespace TalabatAPIs.Controllers
{
    public class ProductsController : APIBaseController
    {
        private readonly IProductService _productService;
        private readonly IMapper mapper;
        //private readonly IGenericRepository<Product> productRepo;
        //private readonly IGenericRepository<ProductType> typeRepo;
        //private readonly IGenericRepository<ProductBrand> brandRepo;

        public ProductsController(
            IProductService productService
            //IGenericRepository<Product> productRepo 
            , IMapper mapper
            ////,IGenericRepository<ProductType> typeRepo 
            ////, IGenericRepository<ProductBrand> BrandRepo
            )
        {
            //this.productRepo = productRepo;
            _productService = productService;
            this.mapper = mapper;
            //this.typeRepo = typeRepo;
            //brandRepo = BrandRepo;
        }
        // Get All Product
        // BaseUrl/api/Products -> Get
        //[Authorize]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Pagination<ProductToReturnDto>>>> GetProducts([FromQuery]ProductSpecParams Params)
        {
            //var Spec = new ProductWithBrandAndTypeSpecifications(Params);
            //var Products = await productRepo.GetAllWithSpecAsync(Spec);
           var products = await _productService.GetProductsAsync(Params);
            var MappedProducts = mapper.Map<IReadOnlyList<Product>, IReadOnlyList< ProductToReturnDto>>(products);
            var Count = await _productService.GetCountAsync(Params);

            return Ok(new Pagination<ProductToReturnDto>(Params.PageSize , Params.PageIndex , MappedProducts , Count));
        }

        // Get by Id

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductToReturnDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var product = await _productService.GetProductAsync(id);
            if (product is null) return NotFound(new ApiResponse(404));
            var MappedProduct = mapper.Map<Product, ProductToReturnDto>(product);
            return Ok(MappedProduct);
        }

        // Get All Types
        // BaseUrl/api/Products/Types
        [HttpGet("Types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes()
        {
            var types = await _productService.GetTypesAsync();

            return Ok(types);
        }

        //Get Brands 
        // BaseUrl/api/Products/Brands

        [HttpGet("Brands")]
        public async  Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var Brands = await _productService.GetBrandsAsync();
            return Ok(Brands);
        }

    }
}
