using DemoVS;

// Configura��o do Builder
var builder = WebApplication.CreateBuilder(args);

// Apos o Builder � poss�vel realizar configura��es como:
// - Configura��o do Pipeline
// - Configura��o de Middleware
// - Configura��o de Services

builder.AddSerilog();
builder.Services.AddControllersWithViews();

// Configura��o da App
var app = builder.Build();

// Configura��o de comportamentos da App
app.UseLogTempo();
app.MapGet("/", () => "Hello World!");
app.MapGet("/teste", () =>
{
    Thread.Sleep(1500);
    return "Teste2";
});

// Ap�s tudo configurado, roda a aplica��o
app.Run();
