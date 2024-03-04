using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Linq;
using TesteApi;
using TesteApi.Models;

public static class ApplicationBuilderExtensions
{
    public static void SeedData(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<Context>();
            var appSettings = services.GetRequiredService<IOptions<AppSettings>>().Value;

            // Certifique-se de que o banco de dados foi criado
            context.Database.EnsureCreated();

            // Se não houver produtos, adicione alguns produtos aleatórios
            if (!context.Products.Any())
            {
                var productGenerator = new ProductGenerator(appSettings.Nomes);
                for (int i = 0; i < 5; i++)
                {
                    var randomProduct = productGenerator.GenerateRandomProduct();
                    context.Products.Add(randomProduct);
                }
                context.SaveChanges();
            }
        }
    }
}

