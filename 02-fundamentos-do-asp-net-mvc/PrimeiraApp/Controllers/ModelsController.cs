using Microsoft.AspNetCore.Mvc;
using PrimeiraApp.Models;

namespace PrimeiraApp.Controllers
{
    public class ModelsController : Controller
    {
        public IActionResult Index()
        {
            var aluno = new Aluno()
            {
                Nome = "a",
                Email = "daniel",
                EmailConfirmacao = "daniel.com"
            };


            if (TryValidateModel(aluno))
            {
                return View(aluno);
            }

            var ms = ModelState;

            var erros = ModelState.Select(modelState => modelState.Value.Errors)
                .Where(error => error.Count > 0)
                .ToList();

            erros.ForEach(erro => Console.WriteLine(erro.First().ErrorMessage));

            return View();
        }
    }
}
