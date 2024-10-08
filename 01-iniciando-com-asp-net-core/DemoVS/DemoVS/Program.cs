using DemoVS;

// Configuração do Builder
var builder = WebApplication.CreateBuilder(args);

// Apos o Builder é possível realizar configurações como:
// - Configuração do Pipeline
// - Configuração de Middleware
// - Configuração de Services

builder.AddSerilog();
builder.Services.AddControllersWithViews();

// Configuração da App
var app = builder.Build();

// Configuração de comportamentos da App
app.UseLogTempo();
app.MapGet("/", () => "Hello World!");
app.MapGet("/teste", () =>
{
    Thread.Sleep(1500);
    return "Teste2";
});

// Após tudo configurado, roda a aplicação
app.Run();
