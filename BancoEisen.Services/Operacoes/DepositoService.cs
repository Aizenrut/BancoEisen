using BancoEisen.Models.Informacoes;
using BancoEisen.Models.Abstracoes;
using BancoEisen.Models.Operacoes;
using System;
using BancoEisen.Data.Repositories;
using System.Threading.Tasks;
using BancoEisen.Data.Models;
using BancoEisen.Data.Services.Interfaces;
using System.Linq;

namespace BancoEisen.Services
{
    public class DepositoService : IDepositoService
    {
        private readonly IContaRepository contaRepository;
        private readonly IOperacaoRepository operacaoRepository;
        private readonly IFiltragemService<Operacao, DepositoFiltro> filtragemService;
        private readonly IOrdenacaoService<Operacao> ordenacaoService;

        public DepositoService(IContaRepository contaRepository,
                                  IOperacaoRepository operacaoRepository,
                                  IFiltragemService<Operacao, DepositoFiltro> filtragemService,
                                  IOrdenacaoService<Operacao> ordenacaoService)
        {
            this.contaRepository = contaRepository;
            this.operacaoRepository = operacaoRepository;
            this.filtragemService = filtragemService;
            this.ordenacaoService = ordenacaoService;
        }

        public Operacao[] Todos(DepositoFiltro filtro, Ordem ordem)
        {
            var query = operacaoRepository.All();
            query = filtragemService.Filtrar(query, filtro);
            query = ordenacaoService.Ordenar(query, ordem);

            return query.ToArray();
        }

        public Operacao Consultar(int depositoId)
        {
            return operacaoRepository.Get(depositoId);
        }

        public async Task<Operacao> Efetivar(OperacaoUnariaInformacoes informacoes)
        {
            ValidarInformacoes(informacoes);

            var deposito = new Deposito(informacoes.Valor, informacoes.Observacao);
            await operacaoRepository.PostAsync(deposito);

            var conta = contaRepository.Get(informacoes.ContaId);
            conta.Saldo += informacoes.Valor;
            conta.Operacoes.Add(deposito);
            await contaRepository.UpdateAsync(conta);

            return deposito;
        }

        private void ValidarInformacoes(OperacaoUnariaInformacoes informacoes)
        {
            if (informacoes.Valor <= 0)
                throw new ArgumentException("O valor a depositar deve ser maior que zero.");

            ValidarContaId(informacoes.ContaId);
        }

        private void ValidarContaId(int contaId)
        {
            if (!contaRepository.Any(contaId))
                throw new ArgumentException($"A conta informada é inválida.");
        }
    }
}
