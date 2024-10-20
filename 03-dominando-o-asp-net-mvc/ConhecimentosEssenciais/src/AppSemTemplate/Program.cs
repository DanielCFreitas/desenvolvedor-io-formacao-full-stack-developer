using AppSemTemplate.Data;
using AppSemTemplate.Extensions;
using AppSemTemplate.Helper;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();


builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

var app = builder.Build();

// Apenas para popular dados no banco para realizar testes
app.UseDbMigrationHelper();

// Configuracao que olha para a pasta "wwwroot" para usar os arquivos estaticos
app.UseStaticFiles();

app.UseRouting();

// Forma de fazer um "transformador de rota" deixando a rota com um padrao especifico
//builder.Services.AddRouting(options =>
//    options.ConstraintMap["slugfy"] = typeof(RouteSlugfyParameterTransformer));

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller:slugfy=Home}/{action:slugfy=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
