using AppSemTemplate.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AppSemTemplate.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ApiConfiguration _apiConfiguration;

        public HomeController(IConfiguration configuration, IOptions<ApiConfiguration> options)
        {
            _configuration = configuration;
            _apiConfiguration = options.Value;
        }

        public IActionResult Index()
        {
            #region Ambiente que esta rodando a aplicacao
            // Retorna o ambiente que esta rodando a aplicacao
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            #endregion


            #region Formas de recuperar informacoes do appsettings.json

            // Primeira forma de recuperar informacoes do appsettings.json
            var apiConfig = new ApiConfiguration();
            _configuration.GetSection(ApiConfiguration.ConfigName).Bind(apiConfig);

            // Segunda forma de recuperar informacoes do appsettings.json
            var user = _configuration[$"{ApiConfiguration.ConfigName}:UserKey"];

            // Terceira forma de recuperar informacoes do appsettings.json usando DI
            var apiConfig2 = _apiConfiguration;

            #endregion

            return View();
        }
    }
}
