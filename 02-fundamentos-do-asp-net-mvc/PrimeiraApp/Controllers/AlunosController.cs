using Microsoft.AspNetCore.Mvc;
using PrimeiraApp.Data;
using PrimeiraApp.Models;

namespace PrimeiraApp.Controllers
{
    public class AlunosController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public AlunosController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,DataNascimento,Email,EmailConfirmacao,Avaliacao,Ativo")] Aluno aluno)
        {
            _appDbContext.Alunos.Add(aluno);
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
