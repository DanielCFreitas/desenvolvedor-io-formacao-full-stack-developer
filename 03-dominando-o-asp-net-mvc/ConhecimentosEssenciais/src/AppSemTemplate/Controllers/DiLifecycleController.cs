using AppSemTemplate.Services;
using Microsoft.AspNetCore.Mvc;

namespace AppSemTemplate.Controllers
{
    [Route("teste-di")]
    public class DiLifecycleController : Controller
    {
        private readonly OperacaoService _operacaoService;
        private readonly OperacaoService _operacaoService2;

        public DiLifecycleController(OperacaoService operacaoService, OperacaoService operacaoService2)
        {
            _operacaoService = operacaoService;
            _operacaoService2 = operacaoService2;
        }

        public string Index()
        {
            return
                "• Transient será trocado sempre que uma nova instância for criada" + Environment.NewLine +
                "• Scoped será sempre igual durante toda a requisição" + Environment.NewLine +
                "• Singleton será sempre igual durante todo o ciclo da aplicação" + Environment.NewLine +
                "• SingletonInstance será sempre igual durante todo o ciclo da aplicação, com o valor que definimos no Program.cs" + Environment.NewLine +

                Environment.NewLine + 
                Environment.NewLine +

                "Primeira instância: " + Environment.NewLine +
                _operacaoService._operacaoTransient.OperacaoId + Environment.NewLine +
                _operacaoService._operacaoScoped.OperacaoId + Environment.NewLine +
                _operacaoService._operacaoSingleton.OperacaoId + Environment.NewLine +
                _operacaoService._operacaoSingletonInstance.OperacaoId + Environment.NewLine +

                Environment.NewLine +
                Environment.NewLine +

                "Segunda instância: " + Environment.NewLine +
                _operacaoService2._operacaoTransient.OperacaoId + Environment.NewLine +
                _operacaoService2._operacaoScoped.OperacaoId + Environment.NewLine +
                _operacaoService2._operacaoSingleton.OperacaoId + Environment.NewLine +
                _operacaoService2._operacaoSingletonInstance.OperacaoId + Environment.NewLine;
        }
    }
}
