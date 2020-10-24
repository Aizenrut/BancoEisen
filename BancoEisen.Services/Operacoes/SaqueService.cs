using BancoEisen.Models.Informacoes;
using BancoEisen.Models.Abstracoes;
using BancoEisen.Models.Operacoes;
using System;
using BancoEisen.Data.Repositories;
using System.Threading.Tasks;
using BancoEisen.Data.Models;
using System.Linq;
using BancoEisen.Data.Services.Interfaces;
using BancoEisen.Models.Cadastros;

namespace BancoEisen.Services
{
    public class SaqueService : ISaqueService
    {
        private readonly IContaRepository contaRepository;
        private readonly IOperacaoRepository operacaoRepository;
        private readonly IFiltragemService<Operacao, SaqueFiltro> filtragemService;
        private readonly IOrdenacaoService<Operacao> ordenacaoService;

        public SaqueService(IContaRepository contaRepository,
                            IOperacaoRepository operacaoRepository,
                            IFiltragemService<Operacao, SaqueFiltro> filtragemService,
                            IOrdenacaoService<Operacao> ordenacaoService)
        {
            this.contaRepository = contaRepository;
            this.operacaoRepository = operacaoRepository;
            this.filtragemService = filtragemService;
            this.ordenacaoService = ordenacaoService;
        }

        public Operacao[] Todos(SaqueFiltro filtro = null, Ordem ordem = null)
        {
            var query = operacaoRepository.All();
            query = filtragemService.Filtrar(query, filtro);
            query = ordenacaoService.Ordenar(query, ordem);

            return query.ToArray();
        }

        public Operacao Consultar(int saqueId)
        {
            return operacaoRepository.Get(saqueId);
        }

        public async Task<Operacao> Efetivar(OperacaoUnariaInformacoes informacoes)
        {
            ValidarInformacoes(informacoes);

            var conta = contaRepository.Get(informacoes.ContaId);

            ValidarSaldo(conta, informacoes);

            var saque = new Saque(informacoes.Valor, informacoes.Observacao);
            await operacaoRepository.PostAsync(saque);

            conta.Saldo -= informacoes.Valor;
            conta.Operacoes.Add(saque);
            await contaRepository.UpdateAsync(conta);

            return saque;
        }

        private void ValidarInformacoes(OperacaoUnariaInformacoes informacoes)
        {
            if (informacoes.Valor <= 0)
                throw new ArgumentException("O valor a sacar deve ser maior que zero.");

            ValidarContaId(informacoes.ContaId);
        }

        private void ValidarContaId(int contaId)
        {
            if (!contaRepository.Any(contaId))
                throw new ArgumentException($"A conta informada é inválida.");
        }

        private void ValidarSaldo(Conta conta, OperacaoUnariaInformacoes informacoes)
        {
            if (conta.Saldo < informacoes.Valor)
                throw new InvalidOperationException("O saldo da conta é insuficiente para realizar a operação.");
        }
    }
}
