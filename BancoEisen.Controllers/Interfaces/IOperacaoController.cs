using BancoEisen.Models.Abstracoes;

namespace BancoEisen.Controllers.Interfaces
{
    public interface IOperacaoController<TInformacoes> : IBancoEisenController<Operacao> where TInformacoes : struct
    {
        Operacao Efetivar(TInformacoes informacoes);
    }
}