using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Repository.Data;
using TalabatAPIs.Errors;

namespace TalabatAPIs.Controllers
{
 
    public class BuggyController : APIBaseController
    {
        private readonly StoreContext dbContext;

        public BuggyController(StoreContext dbContext)
        {
            this.dbContext = dbContext;
        }
        // baseUrl / api / Buggy/NotFound
        [HttpGet("NotFound")]
        public ActionResult GetNotFoundRequest()
        {
            var product = dbContext.Products.Find(100);
            if (product is null) return NotFound(new ApiResponse(404));
            return Ok(product);
        }
        [HttpGet("ServerError")]
        public ActionResult GetServerError()
        {
            var product = dbContext.Products.Find(100);
            var ProductToReturn = product.ToString(); //Error
            // Will Throw exception [Null Reference Exception]
            return Ok(ProductToReturn);
        }

        [HttpGet("BadRequest")]
        public ActionResult GetBadRequest()
        {
            return BadRequest();
        }
        [HttpGet("BadRequest/{id}")]
        public ActionResult GetBadRequest(int id)
        {
            return Ok();
        }

    }
}
