using BancoEisen.API.Models.Resources;
using BancoEisen.API.Models.Operacoes;
using BancoEisen.Models.Abstracoes;
using BancoEisen.Models.Cadastros;

namespace BancoEisen.API.Extensions
{
    public static class EntidadeExtensions
    {
        public static object ToResource(this Entidade entidade)
        {
            if (entidade is Conta conta)
                return new ContaResource(conta);

            if (entidade is Pessoa pessoa)
                return new PessoaResource(pessoa);

            if (entidade is Operacao operacao)
                return new OperacaoResource(operacao);

            return null;
        }
    }
}
