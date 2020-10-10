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
    public class DepositoController : IDepositoController
    {
        private readonly IRepository<Conta> contaRepository;
        private readonly IRepository<Operacao> operacaoRepository;

        public DepositoController(IRepository<Conta> contaRepository, IRepository<Operacao> operacaoRepository)
        {
            this.contaRepository = contaRepository;
            this.operacaoRepository = operacaoRepository;
        }

        public Operacao Consultar(int depositoId)
        {
            return operacaoRepository.Get(depositoId);
        }

        public Operacao Efetivar(OperacaoUnariaInformacoes depositoInformacoes)
        {
            if (depositoInformacoes.Valor <= 0)
                throw new ArgumentException("O valor a depositar deve ser maior que zero.");

            if (!contaRepository.Any(depositoInformacoes.ContaId))
                throw new ArgumentException($"A conta informada é inválida.");

            var deposito = new Deposito(depositoInformacoes.Valor, depositoInformacoes.Observacao);

            var conta = contaRepository.Get(depositoInformacoes.ContaId);
            
            conta.Saldo += depositoInformacoes.Valor;
            conta.Operacoes.Add(deposito);

            contaRepository.Update(conta);

            return operacaoRepository.Post(deposito);
        }

        public Operacao[] Todos()
        {
            return operacaoRepository.Where(operacao => operacao.TipoOperacao == TipoOperacao.Deposito);
        }
    }
}
