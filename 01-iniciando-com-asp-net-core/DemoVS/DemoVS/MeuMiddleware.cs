using Serilog;
using System.Diagnostics;

namespace DemoVS
{
    public class MeuMiddleware
    {
        private readonly RequestDelegate _next;

        public MeuMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            // Faz algo antes

            // Chama o proximo middleware
            await _next(httpContext);

            // Faz algo depois
        }
    }

    public class LogTempoMiddleware
    {
        private readonly RequestDelegate _next;

        public LogTempoMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            // Faz algo antes
            var stopWatch = Stopwatch.StartNew();

            // Chama o proximo middleware
            await _next(httpContext);

            // Faz algo depois
            stopWatch.Stop();

            Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
            Log.Information($"A execução demorou {stopWatch.Elapsed.TotalMilliseconds}(ms) ({stopWatch.Elapsed.TotalSeconds} segundos)");
        }
    }

    public static class SerilogExtensions
    {
        public static void AddSerilog(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog();
        }
    }

    public static class LogTempoMiddlewareExtensions
    {
        public static void UseLogTempo(this WebApplication app)
        {
            app.UseMiddleware<LogTempoMiddleware>();
        }
    }
}
