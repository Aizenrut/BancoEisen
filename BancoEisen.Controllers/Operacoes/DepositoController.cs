using BancoEisen.Controllers.Interfaces;
using BancoEisen.Models.Informacoes;
using BancoEisen.Models.Abstracoes;
using BancoEisen.Models.Operacoes;
using System;
using BancoEisen.Data.Repositorios.Interfaces;
using System.Threading.Tasks;
using BancoEisen.Data.Models.Filtros;
using BancoEisen.Data.Models.Ordens;
using BancoEisen.Data.Services.Interfaces;
using System.Linq;

namespace BancoEisen.Controllers.Operacoes
{
    public class DepositoController : IDepositoController
    {
        private readonly IContaRepositorio contaRepository;
        private readonly IOperacaoRepositorio operacaoRepository;
        private readonly IFiltragemService<Operacao, DepositoFiltro> filtragemService;
        private readonly IOrdenacaoService<Operacao> ordenacaoService;

        public DepositoController(IContaRepositorio contaRepository,
                                  IOperacaoRepositorio operacaoRepository,
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

        public async Task<Operacao> Efetivar(OperacaoUnariaInformacoes depositoInformacoes)
        {
            if (depositoInformacoes.Valor <= 0)
                throw new ArgumentException("O valor a depositar deve ser maior que zero.");

            if (!contaRepository.Any(depositoInformacoes.ContaId))
                throw new ArgumentException($"A conta informada é inválida.");

            var deposito = new Deposito(depositoInformacoes.Valor, depositoInformacoes.Observacao);
            await operacaoRepository.PostAsync(deposito);

            var conta = contaRepository.Get(depositoInformacoes.ContaId);
            conta.Saldo += depositoInformacoes.Valor;
            conta.Operacoes.Add(deposito);
            await contaRepository.UpdateAsync(conta);

            return deposito;
        }
    }
}
