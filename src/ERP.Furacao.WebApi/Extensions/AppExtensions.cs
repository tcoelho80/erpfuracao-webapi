using Microsoft.AspNetCore.Builder;

namespace ERP.Furacao.WebApi.Extensions
{
    public static class AppExtensions
    {
        public static void UseSwaggerExtension(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ERP.Furacao.WebApi");
            });
        }
    }
}
