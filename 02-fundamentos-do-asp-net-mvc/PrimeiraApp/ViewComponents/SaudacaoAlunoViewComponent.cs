using Microsoft.AspNetCore.Mvc;
using PrimeiraApp.Models;

namespace PrimeiraApp.ViewComponents
{
    public class SaudacaoAlunoViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string email)
        {
            // Peguei o aluno no banco de dados !!!
            // Peguei o dado (nome) do aluno que está logado !!!
            // Etc. . .
            var aluno = new Aluno() { Nome = "Daniel", Email = email };

            return View(aluno);
        }
    }
}
