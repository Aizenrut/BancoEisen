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
    public class DepositoServiceTests
    {
        [TestMethod]
        public void Todos_BuscaSemFiltroEOrdem_DeveRetornarAColecaoInicial()
        {
            // Arrange
            var deposito1 = new Deposito { Id = 1 };
            var deposito2 = new Deposito { Id = 2 };
            var deposito3 = new Deposito { Id = 3 };

            var queryDepositos = new List<Deposito>
            {
                deposito1,
                deposito2,
                deposito3
            }.AsQueryable();

            var contaRepository = Substitute.For<IContaRepository>();
            var operacaoRepository = Substitute.For<IOperacaoRepository>();

            var filtragemService = Substitute.For<IFiltragemService<Operacao, DepositoFiltro>>();
            var ordenacaoService = Substitute.For<IOrdenacaoService<Operacao>>();

            filtragemService.Filtrar(Arg.Any<IQueryable<Deposito>>(), null)
                            .Returns(args => (IQueryable<Deposito>)args[0]);

            ordenacaoService.Ordenar(Arg.Any<IQueryable<Deposito>>(), null)
                            .Returns(args => (IQueryable<Deposito>)args[0]);


            var depositoService = new DepositoService(contaRepository, operacaoRepository, filtragemService, ordenacaoService);

            // Act
            var result = depositoService.Todos();

            // Assert
            Assert.AreEqual(3, queryDepositos.Count());

            var listaDepositos = queryDepositos.ToList();

            Assert.AreSame(deposito1, listaDepositos[0]);
            Assert.AreSame(deposito2, listaDepositos[1]);
            Assert.AreSame(deposito3, listaDepositos[2]);
        }

        [TestMethod]
        public void Consultar_IdValido_DeveRetornarODeposito()
        {
            var deposito = new Deposito { Id = 1 };

            var operacaoRepository = Substitute.For<IOperacaoRepository>();

            operacaoRepository.Get(deposito.Id)
                              .Returns(deposito);

            var contaRepository = Substitute.For<IContaRepository>();

            var depositoService = new DepositoService(contaRepository, operacaoRepository, null, null);

            // Act
            var result = depositoService.Consultar(deposito.Id);

            // Assert
            Assert.AreSame(deposito, result);
        }

        [TestMethod]
        public void Consultar_IdInvalido_DeveRetornarNulo()
        {
            var deposito = new Deposito { Id = 1 };

            var operacaoRepository = Substitute.For<IOperacaoRepository>();

            operacaoRepository.Get(deposito.Id)
                              .Returns(deposito);

            var contaRepository = Substitute.For<IContaRepository>();

            var depositoService = new DepositoService(contaRepository, operacaoRepository, null, null);

            // Act
            var result = depositoService.Consultar(2);

            // Assert
            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public async Task Efetivar_ValorZerado_DeveLancarExcecaoValorMaiorQueZero()
        {
            var conta = new Conta { Id = 1 };

            var deposito = new OperacaoUnariaInformacoes(conta.Id, 0);

            var contaRepository = Substitute.For<IContaRepository>();
            var operacaoRepository = Substitute.For<IOperacaoRepository>();

            var depositoService = new DepositoService(contaRepository, operacaoRepository, null, null);

            // Act
            Func<Task> efetivar = () => depositoService.Efetivar(deposito);

            // Assert
            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(efetivar);

            Assert.IsTrue(exception.Message.Contains("O valor a depositar deve ser maior que zero."));
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

            var deposito = new OperacaoUnariaInformacoes(conta2.Id, 100);

            Expression<Func<Conta, bool>> sameIdExpression(int id) => x => x.Id == id;

            var contaRepository = Substitute.For<IContaRepository>();

            contaRepository.Any(Arg.Any<int>())
                           .Returns(args => queryContas.Any(sameIdExpression((int)args[0])));

            var operacaoRepository = Substitute.For<IOperacaoRepository>();

            var depositoService = new DepositoService(contaRepository, operacaoRepository, null, null);

            // Act
            Func<Task> efetivar = () => depositoService.Efetivar(deposito);

            // Assert
            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(efetivar);

            Assert.IsTrue(exception.Message.Contains("A conta informada é inválida."));
        }

        [TestMethod]
        public async Task Efetivar_InformacoesValidas_DeveAdicionarOValorAoSaldoDaConta()
        {
            // Arrange
            var conta = new Conta
            {
                Id = 1,
                Operacoes = new List<Operacao>()
            };

            var deposito = new OperacaoUnariaInformacoes(conta.Id, 100, "Teste unitário");

            var contaRepository = Substitute.For<IContaRepository>();

            contaRepository.Any(Arg.Any<int>())
                           .Returns(true);

            contaRepository.Get(conta.Id)
                           .Returns(conta);

            var operacaoRepository = Substitute.For<IOperacaoRepository>();

            var depositoService = new DepositoService(contaRepository, operacaoRepository, null, null);

            // Act
            await depositoService.Efetivar(deposito);

            // Assert
            Assert.AreEqual(deposito.Valor, conta.Saldo);
            Assert.IsTrue(conta.Operacoes.Any());
            Assert.AreEqual(TipoOperacao.Deposito, conta.Operacoes.First().TipoOperacao);
            Assert.AreEqual(deposito.Valor, conta.Operacoes.First().Valor);
            Assert.AreEqual(deposito.Observacao, conta.Operacoes.First().Observacao);
        }
    }
}
