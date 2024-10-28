using AppSemTemplate.Configuration;
using AppSemTemplate.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace AppSemTemplate.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ApiConfiguration _apiConfiguration;
        private readonly ILogger<HomeController> _logger;
        private readonly IStringLocalizer<HomeController> _localizer;

        public HomeController(IConfiguration configuration,
                              IOptions<ApiConfiguration> options,
                              ILogger<HomeController> logger,
                              IStringLocalizer<HomeController> localizer)
        {
            _configuration = configuration;
            _apiConfiguration = options.Value;
            _logger = logger;
            _localizer = localizer;
        }

        [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Any, NoStore = false)]
        public IActionResult Index()
        {
            // Logando os dados da aplicacao no ElmahIo
            _logger.LogInformation("Information-Teste");
            _logger.LogCritical("Critical-Teste");
            _logger.LogWarning("Warning-Teste");
            _logger.LogError("Error-Teste");

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

            #region Usando outros idiomas

            ViewData["Message"] = _localizer["Seja bem vindo!"];

            #endregion

            // Teste de Response Cache
            ViewData["Horario"] = DateTime.Now;

            return View();
        }

        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );

            return LocalRedirect(returnUrl);
        }

        [Route("teste-erro")]
        public IActionResult TesteErro()
        {
            throw new Exception("Algo errado não estava certo!");

            return View("Index");
        }

        [Route("erro/{id:length(3,3)}")]
        public IActionResult Errors(int id)
        {
            var modelError = new ErrorViewModel();
            modelError.ErrorCode = id;

            if (id == 500)
            {
                modelError.Titulo = "Ocorreu um erro";
                modelError.Mensagem = "Ocorreu um erro! Tente novamente mais tarde ou contate o nosso suporte.";
            }
            else if (id == 404)
            {
                modelError.Titulo = "Ops! Página não encontrada";
                modelError.Mensagem = "A página que está procurando não existe! <br /> Em caso de dúvida entre em contato com o suporte.";
            }
            else if (id == 403)
            {
                modelError.Titulo = "Acesso Negado";
                modelError.Mensagem = "Você não tem permissão para fazer isso.";
            }
            else
            {
                return StatusCode(500);
            }

            return View("Error", modelError);
        }
    }
}
