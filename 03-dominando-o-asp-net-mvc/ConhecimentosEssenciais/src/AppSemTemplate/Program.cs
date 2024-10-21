using AppSemTemplate.Data;
using AppSemTemplate.Helper;
using AppSemTemplate.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Adicionando suporte para mudança de convenção de rota das areas
//builder.Services.Configure<RazorViewEngineOptions>(options =>
//{
//    options.AreaPageViewLocationFormats.Clear();
//    options.AreaPageViewLocationFormats.Add("/Modulos/{2}/Views/{1}/{0}.cshtml");
//    options.AreaPageViewLocationFormats.Add("/Modulos/{2}/Views/Shared/{0}.cshtml");
//    options.AreaPageViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
//});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddTransient<IOperacaoTransient, Operacao>();
builder.Services.AddScoped<IOperacaoScoped, Operacao>();
builder.Services.AddSingleton<IOperacaoSingleton, Operacao>();
builder.Services.AddSingleton<IOperacaoSingletonInstance>(new Operacao(Guid.Empty));
builder.Services.AddTransient<OperacaoService>();

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
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
