using Elmah.Io.AspNetCore;
using Elmah.Io.Extensions.Logging;

namespace AppSemTemplate.Configuration
{
    public static class LoggingConfig
    {
        public static WebApplicationBuilder AddLoggingConfiguration(this WebApplicationBuilder builder)
        {
            //1) Configuracao para logar erros da aplicacao com status code, stacktrace etc...
            builder.Services.Configure<ElmahIoOptions>(builder.Configuration.GetSection("ElmahIo"));
            builder.Services.AddElmahIo();



            //2) Configuracao de Log igual ao que aparece no Console.WriteLine()
            builder.Logging.Services.Configure<ElmahIoProviderOptions>(builder.Configuration.GetSection("ElmahIo"));
            builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
            builder.Logging.AddElmahIo();

            //2.1) Aceitando logs de Warning para "cima"
            builder.Logging.AddFilter<ElmahIoLoggerProvider>(null, LogLevel.Warning);

            return builder;
        }
    }
}
