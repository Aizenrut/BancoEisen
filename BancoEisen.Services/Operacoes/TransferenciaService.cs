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
    public class TransferenciaService : ITransferenciaService
    {
        private readonly IContaRepository contaRepository;
        private readonly IOperacaoRepository operacaoRepository;
        private readonly IDepositoService depositoController;
        private readonly ISaqueService saqueController;
        private readonly IFiltragemService<Operacao, TransferenciaFiltro> filtragemService;
        private readonly IOrdenacaoService<Operacao> ordenacaoService;

        public TransferenciaService(IContaRepository contaRepository,
                                       IOperacaoRepository operacaoRepository,
                                       IDepositoService depositoController,
                                       ISaqueService saqueController,
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

        public async Task<Operacao> Efetivar(OperacaoBinariaInformacoes informacoes)
        {
            ValidarInformacoes(informacoes);

            await saqueController.Efetivar(new OperacaoUnariaInformacoes(informacoes.ContaOrigemId,
                                                                         informacoes.Valor,
                                                                         informacoes.Observacao));

            await depositoController.Efetivar(new OperacaoUnariaInformacoes(informacoes.ContaDestinoId,
                                                                            informacoes.Valor,
                                                                            informacoes.Observacao));

            var transferencia = new Transferencia(informacoes.Valor, informacoes.Observacao);
            await operacaoRepository.PostAsync(transferencia);

            var conta = contaRepository.Get(informacoes.ContaOrigemId);
            conta.Operacoes.Add(transferencia);
            await contaRepository.UpdateAsync(conta);

            return transferencia;
        }

        private void ValidarInformacoes(OperacaoBinariaInformacoes informacoes)
        {
            if (informacoes.Valor <= 0)
                throw new ArgumentException("O valor a transferir deve ser maior que zero.");
        }
    }
}
