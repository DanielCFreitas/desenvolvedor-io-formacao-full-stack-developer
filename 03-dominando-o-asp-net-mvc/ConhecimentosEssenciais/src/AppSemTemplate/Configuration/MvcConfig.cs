using AppSemTemplate.Data;
using AppSemTemplate.Helper;
using AppSemTemplate.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppSemTemplate.Configuration
{
    public static class MvcConfig
    {
        public static WebApplicationBuilder AddMvcConfiguration(this WebApplicationBuilder builder)
        {
            // Configuracoes diferentes para diferentes ambientes appsettings.json
            builder.Configuration
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables();

            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            builder.Services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(60);
                options.ExcludedHosts.Add("example.com");
                options.ExcludedHosts.Add("www.example.com");
            });

            // Adicionando suporte para mudan�a de convencao de rota das areas
            //builder.Services.Configure<RazorViewEngineOptions>(options =>
            //{
            //    options.AreaPageViewLocationFormats.Clear();
            //    options.AreaPageViewLocationFormats.Add("/Modulos/{2}/Views/{1}/{0}.cshtml");
            //    options.AreaPageViewLocationFormats.Add("/Modulos/{2}/Views/Shared/{0}.cshtml");
            //    options.AreaPageViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
            //});

            // Forma de fazer um "transformador de rota" deixando a rota com um padrao especifico
            //builder.Services.AddRouting(options =>
            //    options.ConstraintMap["slugfy"] = typeof(RouteSlugfyParameterTransformer));


            return builder;
        }

        public static WebApplication UseMvcConfiguration(this WebApplication app)
        {
            if (!app.Environment.IsDevelopment())
                app.UseHsts();

            // Redirecionamento para HTTPS
            app.UseHttpsRedirection();

            // Apenas para popular dados no banco para realizar testes
            app.UseDbMigrationHelper();

            // Configuracao que olha para a pasta "wwwroot" para usar os arquivos estaticos
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthorization();


            //app.MapControllerRoute(
            //    name: "default",
            //    pattern: "{controller:slugfy=Home}/{action:slugfy=Index}/{id?}");

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();

            using (var serviceScope = app.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                var singService = services.GetRequiredService<IOperacaoSingleton>();

                Console.WriteLine("Direto do Program.cs: " + singService.OperacaoId);
            }

            return app;
        }
    }
}
