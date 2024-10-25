using AppSemTemplate.Configuration;
using AppSemTemplate.Models;
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
