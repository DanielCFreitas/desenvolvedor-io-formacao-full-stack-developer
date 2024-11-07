using AppSemTemplate.Data;
using AppSemTemplate.Extensions;
using AppSemTemplate.Helper;
using AppSemTemplate.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

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
                .AddEnvironmentVariables()
                .AddUserSecrets(Assembly.GetExecutingAssembly(), true);

            builder.Services.AddResponseCaching();

            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                options.Filters.Add(typeof(FiltroAuditoria));
                MvcOptionsConfig.ConfigurarMensagensDeModelBinding(options.ModelBindingMessageProvider);
            }).AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
              .AddDataAnnotationsLocalization();

            // Data Protection
            builder.Services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo("@/var/data_protection_keys/"))
                .SetApplicationName("MinhaAppMVC");

            // Verifica consentimento do usuario para aceitar cookies 
            builder.Services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
                options.ConsentCookieValue = "true";
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

            // Configura para injetar uma chave especifica do appsettings.json
            builder.Services.Configure<ApiConfiguration>(
                builder.Configuration.GetSection(ApiConfiguration.ConfigName));

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

            builder.Services.AddHostedService<ImageWatermarkService>();

            return builder;
        }

        public static WebApplication UseMvcConfiguration(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // Tratamento de erros 500
                app.UseExceptionHandler("/erro/500");

                // Tratamento de erro com outros status code
                app.UseStatusCodePagesWithRedirects("/erro/{0}");
                app.UseHsts();
            }

            app.UseResponseCaching();

            app.UseGlobalizationConfig();

            // Habilita o ElmahIo para realizar logs da aplicação
            app.UseElmahIo();
            app.UseElmahIoExtensionsLogging();

            // Redirecionamento para HTTPS
            app.UseHttpsRedirection();

            // Apenas para popular dados no banco para realizar testes
            app.UseDbMigrationHelper();

            // Configuracao que olha para a pasta "wwwroot" para usar os arquivos estaticos
            app.UseStaticFiles();

            // Forcar o uso do cookie no pipeline
            app.UseCookiePolicy();

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
