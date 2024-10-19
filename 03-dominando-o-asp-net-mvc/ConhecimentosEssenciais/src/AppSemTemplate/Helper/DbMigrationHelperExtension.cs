using AppSemTemplate.Data;
using AppSemTemplate.Models;
using Microsoft.EntityFrameworkCore;

namespace AppSemTemplate.Helper
{
    public static class DbMigrationHelperExtension
    {
        public static void UseDbMigrationHelper(this WebApplication app)
        {
            DbMigrationHelpers.EnsureSeedData(app).Wait();
        }
    }

    public static class DbMigrationHelpers
    {
        public static async Task EnsureSeedData(WebApplication serviceScope)
        {
            var services = serviceScope.Services.CreateScope().ServiceProvider;
            await EnsureSeedData(services);
        }

        public static async Task EnsureSeedData(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            if (env.IsDevelopment() || env.IsEnvironment("Docker") || env.IsStaging())
            {
                await context.Database.MigrateAsync();
                await EnsureSeedProducts(context);
            }
        }

        private static async Task EnsureSeedProducts(AppDbContext context)
        {
            if (context.Produtos.Any())
                return;

            await context.Produtos.AddAsync(new Produto()
            {
                //Id = 1,
                Nome = "Livro JQuery",
                Imagem = "JQuery.jpg",
                Valor = "50",
            });

            await context.Produtos.AddAsync(new Produto()
            {
                //Id = 2,
                Nome = "Livro HTML",
                Imagem = "HTML.jpg",
                Valor = "100",
            });

            await context.Produtos.AddAsync(new Produto()
            {
                //Id = 3,
                Nome = "Livro Razor",
                Imagem = "Razor.jpg",
                Valor = "150",
            });

            await context.Produtos.AddAsync(new Produto()
            {
                //Id = 4,
                Nome = "Livro CSS",
                Imagem = "CSS.jpg",
                Valor = "200",
            });

            await context.SaveChangesAsync();
        }
    }
}
