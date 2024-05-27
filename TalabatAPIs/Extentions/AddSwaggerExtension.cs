namespace TalabatAPIs.Extentions
{
    public static class AddSwaggerExtension
    {
        public static WebApplication AddSwaggerMiddelwares(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            return app;
        }
    }
}
