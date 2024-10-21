using AppSemTemplate.Services;
using Microsoft.AspNetCore.Mvc;

namespace AppSemTemplate.Controllers
{
    [Route("teste-di")]
    public class DiLifecycleController : Controller
    {
        private readonly IOperacao _operacao;

        public DiLifecycleController(IOperacao operacao)
        {
            _operacao = operacao;
        }

        public IActionResult Index()
        {
            var teste = _operacao;
            return View();
        }
    }
}
