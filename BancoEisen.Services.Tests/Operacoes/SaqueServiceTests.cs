using BancoEisen.Data.Models;
using BancoEisen.Data.Repositories;
using BancoEisen.Data.Services.Interfaces;
using BancoEisen.Models.Abstracoes;
using BancoEisen.Models.Cadastros;
using BancoEisen.Models.Enumeracoes;
using BancoEisen.Models.Informacoes;
using BancoEisen.Models.Operacoes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BancoEisen.Services.Tests.Operacoes
{
    [TestClass]
    public class SaqueServiceTests
    {
        [TestMethod]
        public void Todos_BuscaSemFiltroEOrdem_DeveRetornarAColecaoInicial()
        {
            // Arrange
            var saque1 = new Saque { Id = 1 };
            var saque2 = new Saque { Id = 2 };
            var saque3 = new Saque { Id = 3 };

            var querySaques = new List<Saque>
            {
                saque1,
                saque2,
                saque3
            }.AsQueryable();

            var contaRepository = Substitute.For<IContaRepository>();
            var operacaoRepository = Substitute.For<IOperacaoRepository>();

            var filtragemService = Substitute.For<IFiltragemService<Operacao, SaqueFiltro>>();
            var ordenacaoService = Substitute.For<IOrdenacaoService<Operacao>>();

            filtragemService.Filtrar(Arg.Any<IQueryable<Saque>>(), null)
                            .Returns(args => (IQueryable<Saque>)args[0]);

            ordenacaoService.Ordenar(Arg.Any<IQueryable<Saque>>(), null)
                            .Returns(args => (IQueryable<Saque>)args[0]);


            var saqueService = new SaqueService(contaRepository, operacaoRepository, filtragemService, ordenacaoService);

            // Act
            var result = saqueService.Todos();

            // Assert
            Assert.AreEqual(3, querySaques.Count());

            var listaSaques = querySaques.ToList();

            Assert.AreSame(saque1, listaSaques[0]);
            Assert.AreSame(saque2, listaSaques[1]);
            Assert.AreSame(saque3, listaSaques[2]);
        }

        [TestMethod]
        public void Consultar_IdValido_DeveRetornarOSaque()
        {
            var saque = new Saque { Id = 1 };

            var operacaoRepository = Substitute.For<IOperacaoRepository>();

            operacaoRepository.Get(saque.Id)
                              .Returns(saque);

            var contaRepository = Substitute.For<IContaRepository>();

            var saqueService = new SaqueService(contaRepository, operacaoRepository, null, null);

            // Act
            var result = saqueService.Consultar(saque.Id);

            // Assert
            Assert.AreSame(saque, result);
        }

        [TestMethod]
        public void Consultar_IdInvalido_DeveRetornarNulo()
        {
            var saque = new Saque { Id = 1 };

            var operacaoRepository = Substitute.For<IOperacaoRepository>();

            operacaoRepository.Get(saque.Id)
                              .Returns(saque);

            var contaRepository = Substitute.For<IContaRepository>();

            var saqueService = new SaqueService(contaRepository, operacaoRepository, null, null);

            // Act
            var result = saqueService.Consultar(2);

            // Assert
            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public async Task Efetivar_ValorZerado_DeveLancarExcecaoValorMaiorQueZero()
        {
            var conta = new Conta { Id = 1 };

            var saque = new OperacaoUnariaInformacoes(conta.Id, 0);

            var contaRepository = Substitute.For<IContaRepository>();
            var operacaoRepository = Substitute.For<IOperacaoRepository>();

            var saqueService = new SaqueService(contaRepository, operacaoRepository, null, null);

            // Act
            Func<Task> efetivar = () => saqueService.Efetivar(saque);

            // Assert
            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(efetivar);

            Assert.IsTrue(exception.Message.Contains("O valor a sacar deve ser maior que zero."));
        }

        [TestMethod]
        public async Task Efetivar_ContaInvalida_DeveLancarExcecaoContaInvalida()
        {
            // Arrange
            var conta1 = new Conta { Id = 1 };
            var conta2 = new Conta { Id = 2 };

            var queryContas = new List<Conta>
            {
                conta1
            }.AsQueryable();

            var saque = new OperacaoUnariaInformacoes(conta2.Id, 100);

            Expression<Func<Conta, bool>> sameIdExpression(int id) => x => x.Id == id;

            var contaRepository = Substitute.For<IContaRepository>();

            contaRepository.Any(Arg.Any<int>())
                           .Returns(args => queryContas.Any(sameIdExpression((int)args[0])));

            var operacaoRepository = Substitute.For<IOperacaoRepository>();

            var saqueService = new SaqueService(contaRepository, operacaoRepository, null, null);

            // Act
            Func<Task> efetivar = () => saqueService.Efetivar(saque);

            // Assert
            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(efetivar);

            Assert.IsTrue(exception.Message.Contains("A conta informada é inválida."));
        }

        [TestMethod]
        public async Task Efetivar_SaldoInsuficiente_DeveLancarExcecaoSaldoInsuficiente()
        {
            // Arrange
            var conta = new Conta { Id = 1 };

            var saque = new OperacaoUnariaInformacoes(conta.Id, 100, "Teste unitário");

            var contaRepository = Substitute.For<IContaRepository>();

            contaRepository.Any(Arg.Any<int>())
                           .Returns(true);

            contaRepository.Get(conta.Id)
                           .Returns(conta);

            var operacaoRepository = Substitute.For<IOperacaoRepository>();

            var saqueService = new SaqueService(contaRepository, operacaoRepository, null, null);

            // Act
            Func<Task> efetivar = () => saqueService.Efetivar(saque);

            // Assert
            var exception = await Assert.ThrowsExceptionAsync<InvalidOperationException>(efetivar);

            Assert.IsTrue(exception.Message.Contains("O saldo da conta é insuficiente para realizar a operação."));
        }

        [TestMethod]
        public async Task Efetivar_InformacoesValidas_DeveAdicionarOValorAoSaldoDaConta()
        {
            // Arrange
            var conta = new Conta
            {
                Id = 1,
                Saldo = 100,
                Operacoes = new List<Operacao>()
            };

            var saque = new OperacaoUnariaInformacoes(conta.Id, 100, "Teste unitário");

            var contaRepository = Substitute.For<IContaRepository>();

            contaRepository.Any(Arg.Any<int>())
                           .Returns(true);

            contaRepository.Get(conta.Id)
                           .Returns(conta);

            var operacaoRepository = Substitute.For<IOperacaoRepository>();

            var saqueService = new SaqueService(contaRepository, operacaoRepository, null, null);

            // Act
            await saqueService.Efetivar(saque);

            // Assert
            Assert.AreEqual(0, conta.Saldo);
            Assert.IsTrue(conta.Operacoes.Any());

            Assert.AreEqual(TipoOperacao.Saque, conta.Operacoes.First().TipoOperacao);
            Assert.AreEqual(saque.Valor, conta.Operacoes.First().Valor);
            Assert.AreEqual(saque.Observacao, conta.Operacoes.First().Observacao);
        }
    }
}
