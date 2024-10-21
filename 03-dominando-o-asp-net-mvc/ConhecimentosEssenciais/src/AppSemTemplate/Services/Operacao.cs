namespace AppSemTemplate.Services
{
    public class OperacaoService
    {
        public readonly IOperacaoTransient _operacaoTransient;
        public readonly IOperacaoScoped _operacaoScoped;
        public readonly IOperacaoSingleton _operacaoSingleton;
        public readonly IOperacaoSingletonInstance _operacaoSingletonInstance;

        public OperacaoService(IOperacaoTransient operacaoTransient,
                               IOperacaoScoped operacaoScoped,
                               IOperacaoSingleton operacaoSingleton,
                               IOperacaoSingletonInstance operacaoSingletonInstance)
        {
            _operacaoTransient = operacaoTransient;
            _operacaoScoped = operacaoScoped;
            _operacaoSingleton = operacaoSingleton;
            _operacaoSingletonInstance = operacaoSingletonInstance;
        }
    }

    public class Operacao : IOperacaoTransient,
                            IOperacaoScoped,
                            IOperacaoSingleton,
                            IOperacaoSingletonInstance
    {
        public Guid OperacaoId { get; private set; }

        public Operacao() : this(Guid.NewGuid()) { }

        public Operacao(Guid id)
        {
            OperacaoId = id;
        }
    }

    public interface IOperacao
    {
        Guid OperacaoId { get; }
    }

    public interface IOperacaoTransient : IOperacao { }
    public interface IOperacaoScoped : IOperacao { }
    public interface IOperacaoSingleton : IOperacao { }
    public interface IOperacaoSingletonInstance : IOperacao { }
}
