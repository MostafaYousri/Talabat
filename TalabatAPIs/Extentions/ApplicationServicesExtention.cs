using Microsoft.AspNetCore.Mvc;
using Talabat.Core;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Repository;
using Talabat.Services;
using TalabatAPIs.Errors;
using TalabatAPIs.Helpers;

namespace TalabatAPIs.Extentions
{
    public static  class ApplicationServicesExtention
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services)
        {
            
            Services.AddScoped(typeof(IProductService), typeof(ProductService));
            Services.AddScoped(typeof(IOrderService), typeof(OrderService));
            Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            Services.AddScoped(typeof(IPaymentService), typeof(PaymentService));

            //builder.Services.AddAutoMapper(M =>M.AddProfile(new MappingProfiles()));
            Services.AddAutoMapper(typeof(MappingProfiles));

            //builder.Services.AddScoped<IGenericRepository<Product>, GenericRepository<Product>>();
            //builder.Services.AddScoped<IGenericRepository<ProductBrand>, GenericRepository<ProductBrand>>();
            //Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
           
            Services.AddScoped(typeof(IBasketRepository),typeof( BasketRepository));

            Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    // ModelState => Dic[KeyvaluePair] 
                    // Key => Name of Paramter
                    // Value => Error
                    var errors = actionContext.ModelState.Where(p => p.Value.Errors.Count() > 0)
                                                         .SelectMany(p => p.Value.Errors)
                                                         .Select(E => E.ErrorMessage)
                                                         .ToArray();
                    var apiVaildationErrorResponse = new ApiVaildationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(apiVaildationErrorResponse);
                };
            });
            return Services;
        }
    }
}
