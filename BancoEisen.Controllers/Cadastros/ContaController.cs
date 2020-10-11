using BancoEisen.Controllers.Interfaces;
using BancoEisen.Data.Models.Filtros;
using BancoEisen.Data.Models.Ordens;
using BancoEisen.Data.Repositorios.Interfaces;
using BancoEisen.Models.Cadastros;
using BancoEisen.Models.Informacoes;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BancoEisen.Controllers.Cadastros
{
    public class ContaController : IContaController
    {
        private readonly IContaRepositorio contaRepository;
        private readonly IPessoaRepositorio pessoaRepository;

        public ContaController(IContaRepositorio contaRepositorio, IPessoaRepositorio pessoaRepository)
        {
            this.contaRepository = contaRepositorio;
            this.pessoaRepository = pessoaRepository;
        }

        public Conta[] Todos(ContaFiltro filtro = null, Ordem ordem = null)
        {
            var query = contaRepository.All();
            query = contaRepository.Filtrar(query, filtro);
            query = contaRepository.Ordenar(query, ordem);

            return query.ToArray();
        }

        public Conta Consultar(int id)
        {
            return contaRepository.Get(id);
        }

        public async Task<Conta> Cadastrar(ContaInformacoes informacoes)
        {
            if (!pessoaRepository.Any(informacoes.TitularId))
                throw new ArgumentException("O titular informado é inválido.");

            var conta = new Conta(informacoes.Agencia,
                                  informacoes.Numero,
                                  informacoes.Digito,
                                  informacoes.TitularId);

            await contaRepository.PostAsync(conta);

            return conta;
        }

        public async Task Alterar(Conta conta)
        {
            if (!contaRepository.Any(conta.Id))
                throw new ArgumentException("A pessoa informada é inválida.");

            var contaSalva = contaRepository.Get(conta.Id);

            contaSalva.Agencia = conta.Agencia;
            contaSalva.Numero = conta.Numero;
            contaSalva.Digito = conta.Digito;

            await contaRepository.UpdateAsync(contaSalva);
        }

        public async Task Remover(int id)
        {
            if (!contaRepository.Any(id))
                throw new ArgumentException("A conta informada é inválida.");

            var conta = contaRepository.Get(id);

            await contaRepository.DeleteAsync(conta);
        }
    }
}
