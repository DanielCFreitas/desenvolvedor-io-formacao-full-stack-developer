using AppSemTemplate.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder
        .AddGlobalizationConfig()
        .AddLoggingConfiguration()
        .AddMvcConfiguration()
        .AddIdentityConfiguration()
        .AddDependencyInjectionConfiguration();

var app = builder.Build();
app.UseMvcConfiguration();
app.Run();
