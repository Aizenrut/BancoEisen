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
    public class TransferenciaController : ITransferenciaController
    {
        private readonly IContaRepositorio contaRepository;
        private readonly IOperacaoRepositorio operacaoRepository;
        private readonly IDepositoController depositoController;
        private readonly ISaqueController saqueController;
        private readonly IFiltragemService<Operacao, TransferenciaFiltro> filtragemService;
        private readonly IOrdenacaoService<Operacao> ordenacaoService;

        public TransferenciaController(IContaRepositorio contaRepository,
                                       IOperacaoRepositorio operacaoRepository,
                                       IDepositoController depositoController,
                                       ISaqueController saqueController,
                                       IFiltragemService<Operacao, TransferenciaFiltro> filtragemService,
                                       IOrdenacaoService<Operacao> ordenacaoService)
        {
            this.contaRepository = contaRepository;
            this.operacaoRepository = operacaoRepository;
            this.depositoController = depositoController;
            this.saqueController = saqueController;
            this.filtragemService = filtragemService;
            this.ordenacaoService = ordenacaoService;
        }

        public Operacao[] Todos(TransferenciaFiltro filtro, Ordem ordem)
        {
            var query = operacaoRepository.All();
            query = filtragemService.Filtrar(query, filtro);
            query = ordenacaoService.Ordenar(query, ordem);

            return query.ToArray();
        }

        public Operacao Consultar(int transferenciaId)
        {
            return operacaoRepository.Get(transferenciaId);
        }

        public async Task<Operacao> Efetivar(OperacaoBinariaInformacoes transferenciaInformacoes)
        {
            if (transferenciaInformacoes.Valor <= 0)
                throw new ArgumentException("O valor a transferir deve ser maior que zero.");

            await saqueController.Efetivar(new OperacaoUnariaInformacoes(transferenciaInformacoes.ContaOrigemId,
                                                                         transferenciaInformacoes.Valor,
                                                                         transferenciaInformacoes.Observacao));

            await depositoController.Efetivar(new OperacaoUnariaInformacoes(transferenciaInformacoes.ContaDestinoId,
                                                                            transferenciaInformacoes.Valor,
                                                                            transferenciaInformacoes.Observacao));

            var transferencia = new Transferencia(transferenciaInformacoes.Valor, transferenciaInformacoes.Observacao);
            await operacaoRepository.PostAsync(transferencia);

            var conta = contaRepository.Get(transferenciaInformacoes.ContaOrigemId);
            conta.Operacoes.Add(transferencia);
            await contaRepository.UpdateAsync(conta);

            return transferencia;
        }
    }
}
