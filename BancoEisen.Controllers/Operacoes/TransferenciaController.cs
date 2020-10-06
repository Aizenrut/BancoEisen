using BancoEisen.Controllers.Interfaces;
using BancoEisen.Models.Informacoes;
using BancoEisen.Models.Enumeracoes;
using BancoEisen.Models.Abstracoes;
using BancoEisen.Models.Cadastros;
using BancoEisen.Models.Operacoes;
using BancoEisen.Data;
using System;

namespace BancoEisen.Controllers.Operacoes
{
    public class TransferenciaController : ITransferenciaController
    {
        private IRepository<Conta> contaRepository;
        private IRepository<Operacao> operacaoRepository;
        private IDepositoController depositoController;
        private ISaqueController saqueController;

        public TransferenciaController(IRepository<Conta> contaRepository,
                                       IRepository<Operacao> operacaoRepository,
                                       IDepositoController depositoController,
                                       ISaqueController saquecController)
        {
            this.contaRepository = contaRepository;
            this.operacaoRepository = operacaoRepository;
            this.depositoController = depositoController;
            this.saqueController = saquecController;
        }

        public Operacao Consultar(int transferenciaId)
        {
            return operacaoRepository.Get(transferenciaId);
        }

        public Operacao Efetivar(OperacaoBinariaInformacoes transferenciaInformacoes)
        {
            if (transferenciaInformacoes.Valor <= 0)
                throw new ArgumentException("O valor a transferir deve ser maior que zero");

            saqueController.Efetivar(new OperacaoUnariaInformacoes(transferenciaInformacoes.ContaOrigemId,
                                                                   transferenciaInformacoes.Valor,
                                                                   transferenciaInformacoes.Observacao));

            depositoController.Efetivar(new OperacaoUnariaInformacoes(transferenciaInformacoes.ContaDestinoId,
                                                                      transferenciaInformacoes.Valor,
                                                                      transferenciaInformacoes.Observacao));

            var transferencia = new Transferencia(transferenciaInformacoes.Valor, transferenciaInformacoes.Observacao);

            var conta = contaRepository.Get(transferenciaInformacoes.ContaOrigemId);
            conta.Operacoes.Add(transferencia);

            return operacaoRepository.Post(transferencia);
        }

        public Operacao[] Todos()
        {
            return operacaoRepository.Where(operacao => operacao.TipoOperacao == TipoOperacao.Transferencia);
        }
    }
}
