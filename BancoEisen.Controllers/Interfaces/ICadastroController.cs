using BancoEisen.Models.Abstracoes;

namespace BancoEisen.Controllers.Interfaces
{
    public interface ICadastroController<TInformacoes, TEntidade> : IBancoEisenController<TEntidade> where TEntidade : Entidade
    {
        TEntidade Cadastrar(TInformacoes informacoes);
        void Alterar(TEntidade entidade);
        void Remover(int id);
    }
}
