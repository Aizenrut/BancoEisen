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
using System.Threading.Tasks;

namespace BancoEisen.Services.Tests.Operacoes
{
    [TestClass]
    public class TransferenciaServiceTests
    {
        [TestMethod]
        public void Todos_BuscaSemFiltroEOrdem_DeveRetornarAColecaoInicial()
        {
            // Arrange
            var transferencia1 = new Transferencia { Id = 1 };
            var transferencia2 = new Transferencia { Id = 2 };
            var transferencia3 = new Transferencia { Id = 3 };

            var queryTransferencias = new List<Transferencia>
            {
                transferencia1,
                transferencia2,
                transferencia3
            }.AsQueryable();

            var contaRepository = Substitute.For<IContaRepository>();
            var operacaoRepository = Substitute.For<IOperacaoRepository>();

            var filtragemService = Substitute.For<IFiltragemService<Operacao, TransferenciaFiltro>>();
            var ordenacaoService = Substitute.For<IOrdenacaoService<Operacao>>();

            filtragemService.Filtrar(Arg.Any<IQueryable<Transferencia>>(), null)
                            .Returns(args => (IQueryable<Transferencia>)args[0]);

            ordenacaoService.Ordenar(Arg.Any<IQueryable<Transferencia>>(), null)
                            .Returns(args => (IQueryable<Transferencia>)args[0]);

            var depositoService = Substitute.For<IDepositoService>();
            var saqueService = Substitute.For<ISaqueService>();

            var transferenciaService = new TransferenciaService(contaRepository,
                                                                operacaoRepository, 
                                                                depositoService, 
                                                                saqueService,
                                                                filtragemService,
                                                                ordenacaoService);

            // Act
            var result = transferenciaService.Todos();

            // Assert
            Assert.AreEqual(3, queryTransferencias.Count());

            var listaTransferencias = queryTransferencias.ToList();

            Assert.AreSame(transferencia1, listaTransferencias[0]);
            Assert.AreSame(transferencia2, listaTransferencias[1]);
            Assert.AreSame(transferencia3, listaTransferencias[2]);
        }

        [TestMethod]
        public void Consultar_IdValido_DeveRetornarATransferencia()
        {
            var transferencia = new Transferencia { Id = 1 };

            var operacaoRepository = Substitute.For<IOperacaoRepository>();

            operacaoRepository.Get(transferencia.Id)
                              .Returns(transferencia);

            var contaRepository = Substitute.For<IContaRepository>();

            var depositoService = Substitute.For<IDepositoService>();
            var saqueService = Substitute.For<ISaqueService>();

            var transferenciaService = new TransferenciaService(contaRepository,
                                                                operacaoRepository,
                                                                depositoService,
                                                                saqueService,
                                                                null,
                                                                null);

            // Act
            var result = transferenciaService.Consultar(transferencia.Id);

            // Assert
            Assert.AreSame(transferencia, result);
        }

        [TestMethod]
        public void Consultar_IdInvalido_DeveRetornarNulo()
        {
            var transferencia = new Transferencia { Id = 1 };

            var operacaoRepository = Substitute.For<IOperacaoRepository>();

            operacaoRepository.Get(transferencia.Id)
                              .Returns(transferencia);

            var contaRepository = Substitute.For<IContaRepository>();

            var depositoService = Substitute.For<IDepositoService>();
            var saqueService = Substitute.For<ISaqueService>();

            var transferenciaService = new TransferenciaService(contaRepository,
                                                                operacaoRepository,
                                                                depositoService,
                                                                saqueService,
                                                                null,
                                                                null);

            // Act
            var result = transferenciaService.Consultar(2);

            // Assert
            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public async Task Efetivar_ValorZerado_DeveLancarExcecaoValorMaiorQueZero()
        {
            var contaOrigem = new Conta { Id = 1 };
            var contaDestino = new Conta { Id = 2 };

            var transferencia = new OperacaoBinariaInformacoes(contaOrigem.Id, contaDestino.Id, 0);

            var contaRepository = Substitute.For<IContaRepository>();
            var operacaoRepository = Substitute.For<IOperacaoRepository>();

            var depositoService = Substitute.For<IDepositoService>();
            var saqueService = Substitute.For<ISaqueService>();

            var transferenciaService = new TransferenciaService(contaRepository,
                                                                operacaoRepository,
                                                                depositoService,
                                                                saqueService,
                                                                null,
                                                                null);

            // Act
            Func<Task> efetivar = () => transferenciaService.Efetivar(transferencia);

            // Assert
            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(efetivar);

            Assert.IsTrue(exception.Message.Contains("O valor a transferir deve ser maior que zero."));
        }

        [TestMethod]
        public async Task Efetivar_ContaOrigemInvalida_DeveLancarExcecaoContaInvalida()
        {
            // Arrange
            var transferencia = new OperacaoBinariaInformacoes(1, 2, 100);

            var contaRepository = Substitute.For<IContaRepository>();

            contaRepository.Any(Arg.Any<int>())
                           .Returns(false);

            var operacaoRepository = Substitute.For<IOperacaoRepository>();

            var depositoService = Substitute.For<IDepositoService>();
            var saqueService = new SaqueService(contaRepository, operacaoRepository, null, null);

            var transferenciaService = new TransferenciaService(contaRepository,
                                                                operacaoRepository,
                                                                depositoService,
                                                                saqueService,
                                                                null,
                                                                null);

            // Act
            Func<Task> efetivar = () => transferenciaService.Efetivar(transferencia);

            // Assert
            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(efetivar);

            Assert.IsTrue(exception.Message.Contains("A conta informada é inválida."));
        }

        [TestMethod]
        public async Task Efetivar_SaldoInsuficiente_DeveLancarExcecaoSaldoInsuficiente()
        {
            // Arrange
            var conta = new Conta { Id = 1 };

            var transferencia = new OperacaoBinariaInformacoes(1, 2, 100);

            var contaRepository = Substitute.For<IContaRepository>();

            contaRepository.Any(Arg.Any<int>())
                           .Returns(true);

            contaRepository.Get(conta.Id)
                           .Returns(conta);

            var operacaoRepository = Substitute.For<IOperacaoRepository>();

            var depositoService = Substitute.For<IDepositoService>();
            var saqueService = new SaqueService(contaRepository, operacaoRepository, null, null);

            var transferenciaService = new TransferenciaService(contaRepository,
                                                                operacaoRepository,
                                                                depositoService,
                                                                saqueService,
                                                                null,
                                                                null);

            // Act
            Func<Task> efetivar = () => transferenciaService.Efetivar(transferencia);

            // Assert
            var exception = await Assert.ThrowsExceptionAsync<InvalidOperationException>(efetivar);

            Assert.IsTrue(exception.Message.Contains("O saldo da conta é insuficiente para realizar a operação."));
        }

        [TestMethod]
        public async Task Efetivar_ContaDestinoInvalida_DeveLancarExcecaoContaInvalida()
        {
            // Arrange
            var contaOrigem = new Conta
            {
                Id = 1,
                Saldo = 100
            };

            var contaDestino = new Conta 
            { 
                Id = 2
            };

            var transferencia = new OperacaoBinariaInformacoes(contaOrigem.Id, contaDestino.Id, 100);

            var contaRepository = Substitute.For<IContaRepository>();

            contaRepository.Any(contaOrigem.Id)
                           .Returns(true);

            contaRepository.Get(contaOrigem.Id)
                           .Returns(contaOrigem);

            contaRepository.Any(contaDestino.Id)
                           .Returns(false);

            var operacaoRepository = Substitute.For<IOperacaoRepository>();

            var depositoService = new DepositoService(contaRepository, operacaoRepository, null, null);
            var saqueService = new SaqueService(contaRepository, operacaoRepository, null, null);

            var transferenciaService = new TransferenciaService(contaRepository,
                                                                operacaoRepository,
                                                                depositoService,
                                                                saqueService,
                                                                null,
                                                                null);

            // Act
            Func<Task> efetivar = () => transferenciaService.Efetivar(transferencia);

            // Assert
            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(efetivar);

            Assert.IsTrue(exception.Message.Contains("A conta informada é inválida."));
        }

        [TestMethod]
        public async Task Efetivar_Validas_DeveTransferirERegistrarOperacoes()
        {
            // Arrange
            var contaOrigem = new Conta
            {
                Id = 1,
                Saldo = 100,
                Operacoes = new List<Operacao>()
            };

            var contaDestino = new Conta
            {
                Id = 2,
                Operacoes = new List<Operacao>()
            };

            var transferencia = new OperacaoBinariaInformacoes(contaOrigem.Id, contaDestino.Id, 100, "Teste unitário");

            var contaRepository = Substitute.For<IContaRepository>();

            contaRepository.Any(contaOrigem.Id)
                           .Returns(true);

            contaRepository.Get(contaOrigem.Id)
                           .Returns(contaOrigem);

            contaRepository.Any(contaDestino.Id)
                           .Returns(true);

            contaRepository.Get(contaDestino.Id)
                           .Returns(contaDestino);

            var operacaoRepository = Substitute.For<IOperacaoRepository>();

            var depositoService = new DepositoService(contaRepository, operacaoRepository, null, null);
            var saqueService = new SaqueService(contaRepository, operacaoRepository, null, null);

            var transferenciaService = new TransferenciaService(contaRepository,
                                                                operacaoRepository,
                                                                depositoService,
                                                                saqueService,
                                                                null,
                                                                null);

            // Act
            await transferenciaService.Efetivar(transferencia);

            // Assert
            Assert.AreEqual(0, contaOrigem.Saldo);
            Assert.AreEqual(2, contaOrigem.Operacoes.Count);

            var contaOrigemOperacoes = contaOrigem.Operacoes.ToArray();

            Assert.AreEqual(TipoOperacao.Saque, contaOrigemOperacoes[0].TipoOperacao);
            Assert.AreEqual(transferencia.Valor, contaOrigemOperacoes[0].Valor);
            Assert.AreEqual(transferencia.Observacao, contaOrigemOperacoes[0].Observacao);

            Assert.AreEqual(TipoOperacao.Transferencia, contaOrigemOperacoes[1].TipoOperacao);
            Assert.AreEqual(transferencia.Valor, contaOrigemOperacoes[1].Valor);
            Assert.AreEqual(transferencia.Observacao, contaOrigemOperacoes[1].Observacao);

            Assert.AreEqual(transferencia.Valor, contaDestino.Saldo);
            Assert.IsTrue(contaOrigem.Operacoes.Any());

            Assert.AreEqual(TipoOperacao.Deposito, contaDestino.Operacoes.First().TipoOperacao);
            Assert.AreEqual(transferencia.Valor, contaDestino.Operacoes.First().Valor);
            Assert.AreEqual(transferencia.Observacao, contaDestino.Operacoes.First().Observacao);
        }
    }
}
