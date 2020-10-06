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
    public class SaqueController : ISaqueController
    {
        private IRepository<Conta> contaRepository;
        private IRepository<Operacao> operacaoRepository;

        public SaqueController(IRepository<Conta> contaRepository, IRepository<Operacao> operacaoRepository)
        {
            this.contaRepository = contaRepository;
            this.operacaoRepository = operacaoRepository;
        }

        public Operacao Consultar(int saqueId)
        {
            return operacaoRepository.Get(saqueId);
        }

        public Operacao Efetivar(OperacaoUnariaInformacoes saqueInformacoes)
        {
            if (saqueInformacoes.Valor <= 0)
                throw new ArgumentException("O valor a sacar deve ser maior que zero");

            if (!contaRepository.Any(saqueInformacoes.ContaId))
                throw new ArgumentException($"A conta informada é inválida");

            var conta = contaRepository.Get(saqueInformacoes.ContaId);

            if (conta.Saldo < saqueInformacoes.Valor)
                throw new ArgumentException("O saldo da conta é insuficiente para realizar a operação");

            var saque = new Saque(saqueInformacoes.Valor, saqueInformacoes.Observacao);

            conta.Saldo -= saqueInformacoes.Valor;
            conta.Operacoes.Add(saque);

            contaRepository.Update(conta);

            return operacaoRepository.Post(saque);
        }

        public Operacao[] Todos()
        {
            return operacaoRepository.Where(operacao => operacao.TipoOperacao == TipoOperacao.Saque);
        }
    }
}
