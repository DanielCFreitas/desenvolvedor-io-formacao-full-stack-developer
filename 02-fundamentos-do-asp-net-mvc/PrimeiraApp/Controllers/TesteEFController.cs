using Microsoft.AspNetCore.Mvc;
using PrimeiraApp.Data;
using PrimeiraApp.Models;

namespace PrimeiraApp.Controllers
{
    public class TesteEFController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public TesteEFController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IActionResult Index()
        {
            // Adicionar
            var aluno = new Aluno()
            {
                Nome = "Daniel",
                Email = "daniel.exatas@gmail.com",
                DataNascimento = new DateTime(1992, 4, 2),
                Avaliacao = 5,
                Ativo = true
            };

            _appDbContext.Alunos.Add(aluno);
            _appDbContext.SaveChanges();

            // Consulta
            var alunoChange = _appDbContext.Alunos.Where(aluno => aluno.Nome == "Daniel").FirstOrDefault();

            // Atualiza
            alunoChange.Nome = "Daniel Corrêa";
            _appDbContext.Alunos.Update(alunoChange);
            _appDbContext.SaveChanges();

            // Exclui
            _appDbContext.Alunos.Remove(alunoChange);
            _appDbContext.SaveChanges();

            return View();
        }
    }
}
