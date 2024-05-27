using System.Net;
using System.Text.Json;
using TalabatAPIs.Errors;

namespace TalabatAPIs.MiddelWares
{
    public class ExceptionMiddelWare
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddelWare> logger;
        private readonly IHostEnvironment env;

        public ExceptionMiddelWare(RequestDelegate Next , ILogger<ExceptionMiddelWare> logger, IHostEnvironment env)
        {
            next = Next;
            this.logger = logger;
            this.env = env;
        }
        // InvokeAsync
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (Exception ex )
            {

                logger.LogError(ex, ex.Message);
                // Production => log ex in Database
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

                //if(env.IsDevelopment())
                //{
                //    var Response = new ApiExceptionResponse((int)HttpStatusCode.InternalServerError,
                //        ex.Message, ex.StackTrace.ToString());
                //}
                //else
                //{
                //    var Response = new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);
                //}

                var Response = env.IsDevelopment() ? new ApiExceptionResponse((int)HttpStatusCode.InternalServerError,
                     ex.Message, ex.StackTrace.ToString()) 
                    : new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);
                var Options = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                var JsonResponse = JsonSerializer.Serialize(Response , Options);
               await context.Response.WriteAsync(JsonResponse);
            }
        }
    }
}
