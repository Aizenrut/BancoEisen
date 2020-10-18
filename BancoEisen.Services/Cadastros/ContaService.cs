using BancoEisen.Data.Models;
using BancoEisen.Data.Repositories;
using BancoEisen.Models.Cadastros;
using BancoEisen.Models.Informacoes;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BancoEisen.Services
{
    public class ContaService : IContaService
    {
        private readonly IContaRepository contaRepository;
        private readonly IPessoaRepository pessoaRepository;

        public ContaService(IContaRepository contaRepositorio, IPessoaRepository pessoaRepository)
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
            ValidarInformacoes(informacoes);

            var conta = new Conta(informacoes.Agencia,
                                  informacoes.Numero,
                                  informacoes.Digito,
                                  informacoes.TitularId);

            await contaRepository.PostAsync(conta);

            return conta;
        }

        public async Task Alterar(Conta conta)
        {
            ValidarConta(conta);

            var contaSalva = contaRepository.Get(conta.Id);
            
            if (conta.Agencia > 0)
                contaSalva.Agencia = conta.Agencia;

            if (conta.Numero > 0)
                contaSalva.Numero = conta.Numero;

            if (conta.Digito > 0)
                contaSalva.Digito = conta.Digito;

            await contaRepository.UpdateAsync(contaSalva);
        }

        public async Task Remover(int id)
        {
            ValidarId(id);

            var conta = contaRepository.Get(id);

            await contaRepository.DeleteAsync(conta);
        }

        private void ValidarInformacoes(ContaInformacoes informacoes)
        {
            if (!pessoaRepository.Any(informacoes.TitularId))
                throw new ArgumentException("O titular informado é inválido.");
        }

        private void ValidarConta(Conta conta)
        {
            if (conta == null)
                throw new ArgumentException("A conta informada é inválida.");

            ValidarId(conta.Id);
        }

        private void ValidarId(int id)
        {
            if (!contaRepository.Any(id))
                throw new ArgumentException("A conta informada é inválida.");
        }
    }
}
