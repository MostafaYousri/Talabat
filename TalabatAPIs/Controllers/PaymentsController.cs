using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Services;
using TalabatAPIs.Errors;
using Stripe;
using Talabat.Core.Entities.Order_Aggregate;

namespace TalabatAPIs.Controllers
{
    
    public class PaymentsController : APIBaseController
    {

        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentsController> _logger;
        private const string _whSecret = "whsec_a3ce7d1d8ed779c7ce22af6e0042f0c0a55a200c81c3ed003378f30a13998687";

        public PaymentsController(IPaymentService paymentService,ILogger<PaymentsController>logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }
        [Authorize]
        [ProducesResponseType(typeof(CustomerBasket),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);

            if (basket is null) return BadRequest(new ApiResponse(400, "An Error With Your Basket"));
            return Ok(basket);
         }
        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], _whSecret);
                var paymentIntent =(PaymentIntent) stripeEvent.Data.Object;

                Order order;
                switch (stripeEvent.Type) 
                {
                    case Events.PaymentIntentSucceeded:
                        order = await _paymentService.UpdatePaymentIntentToSucceededOrFailed(paymentIntent.Id,true);
                        _logger.LogInformation("Payment Is Succeeded ya Hamada",paymentIntent.Id);
                        break;
                    case Events.PaymentIntentPaymentFailed:
                        order =  await _paymentService.UpdatePaymentIntentToSucceededOrFailed(paymentIntent.Id, false);
                        _logger.LogInformation("Payment Is Failed ya Hamada", paymentIntent.Id);

                        break;
                    

                }
   
                return new EmptyResult();
            
        }
    }
}
