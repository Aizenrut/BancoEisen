using BancoEisen.Data.Repositories;
using BancoEisen.Models.Cadastros;
using BancoEisen.Models.Informacoes;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BancoEisen.Services.Tests.Cadastros
{
    [TestClass]
    public class ContaServiceTests
    {
        [TestMethod]
        public void Todos_BuscaSemFiltroEOrdem_DeveRetornarAColecaoInicial()
        {
            // Arrange
            var conta1 = new Conta { Id = 1 };
            var conta2 = new Conta { Id = 2 };
            var conta3 = new Conta { Id = 3 };

            var queryContas = new List<Conta>
            {
                conta1,
                conta2,
                conta3
            }.AsQueryable();

            var contaRepository = Substitute.For<IContaRepository>();
            
            contaRepository.Filtrar(Arg.Any<IQueryable<Conta>>(), null)
                           .Returns(args => (IQueryable<Conta>)args[0]);

            contaRepository.Ordenar(Arg.Any<IQueryable<Conta>>(), null)
                           .Returns(args => (IQueryable<Conta>)args[0]);

            var pessoaRepository = Substitute.For<IPessoaRepository>();

            var contaService = new ContaService(contaRepository, pessoaRepository);

            // Act
            var result = contaService.Todos();

            // Assert
            Assert.AreEqual(3, queryContas.Count());

            var listaContas = queryContas.ToList();

            Assert.AreSame(conta1, listaContas[0]);
            Assert.AreSame(conta2, listaContas[1]);
            Assert.AreSame(conta3, listaContas[2]);
        }

        [TestMethod]
        public async Task Cadastrar_TitularInvalido_DeveLancarExcecaoTitularInvalido()
        {
            // Arrange
            var conta = new ContaInformacoes { TitularId = 1 };

            var contaRepository = Substitute.For<IContaRepository>();
            var pessoaRepository = Substitute.For<IPessoaRepository>();

            pessoaRepository.Any(conta.TitularId)
                            .Returns(false);

            var contaService = new ContaService(contaRepository, pessoaRepository);

            // Act
            Func<Task> cadastrar = () => contaService.Cadastrar(conta);

            // Assert
            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(cadastrar);

            Assert.IsTrue(exception.Message.Contains("O titular informado é inválido."));
        }

        [TestMethod]
        public async Task Cadastrar_InformacoesValidas_DeveCadastrarERetornarAConta()
        {
            // Arrange
            var conta = new ContaInformacoes
            {
                Agencia = 1,
                Numero = 1,
                Digito = 1,
                TitularId = 1
            };

            var contaRepository = Substitute.For<IContaRepository>();
            var pessoaRepository = Substitute.For<IPessoaRepository>();

            pessoaRepository.Any(conta.TitularId)
                            .Returns(true);

            var contaService = new ContaService(contaRepository, pessoaRepository);

            // Act
            var result = await contaService.Cadastrar(conta);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(conta.Agencia, result.Agencia);
            Assert.AreEqual(conta.Numero, result.Numero);
            Assert.AreEqual(conta.Digito, result.Digito);
            Assert.AreEqual(conta.TitularId, result.TitularId);
        }

        [TestMethod]
        public void Consultar_IdValido_DeveRetornarAConta()
        {
            // Arrange
            var conta = new Conta { Id = 1 };

            var contaRepository = Substitute.For<IContaRepository>();
            
            contaRepository.Get(conta.Id)
                           .Returns(conta);

            var pessoaRepository = Substitute.For<IPessoaRepository>();

            var contaService = new ContaService(contaRepository, pessoaRepository);

            // Act
            var result = contaService.Consultar(conta.Id);

            // Assert
            Assert.AreSame(conta, result);
        }

        [TestMethod]
        public void Consultar_IdInvalido_DeveRetornarNulo()
        {
            // Arrange
            var conta = new Conta { Id = 1 };

            var contaRepository = Substitute.For<IContaRepository>();

            contaRepository.Get(conta.Id)
                           .Returns(conta);

            var pessoaRepository = Substitute.For<IPessoaRepository>();

            var contaService = new ContaService(contaRepository, pessoaRepository);

            // Act
            var result = contaService.Consultar(2);

            // Assert
            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public async Task Alterar_ContaNula_DeveLancarExcecaoContaInvalida()
        {
            // Arrange
            var contaRepository = Substitute.For<IContaRepository>();
            var pessoaRepository = Substitute.For<IPessoaRepository>();

            var contaService = new ContaService(contaRepository, pessoaRepository);

            // Act
            Func<Task> alterar = () => contaService.Alterar(null);

            // Assert
            var exception = await Assert.ThrowsExceptionAsync<ArgumentNullException>(alterar);

            Assert.IsTrue(exception.Message.Contains("A conta informada é inválida."));
        }

        [TestMethod]
        public async Task Alterar_IdInvalido_DeveLancarExcecaoContaInvalida()
        {
            // Arrange
            var conta1 = new Conta { Id = 1 };
            var conta2 = new Conta { Id = 2 };

            var queryContas = new List<Conta>
            {
                conta1,
            }.AsQueryable();

            Expression<Func<Conta, bool>> sameIdExpression(int id) => x => x.Id == id;

            var contaRepository = Substitute.For<IContaRepository>();

            contaRepository.Any(Arg.Any<int>())
                           .Returns(args => queryContas.Any(sameIdExpression((int)args[0])));

            var pessoaRepository = Substitute.For<IPessoaRepository>();

            var contaService = new ContaService(contaRepository, pessoaRepository);

            // Act
            Func<Task> alterar = () => contaService.Alterar(conta2);

            // Assert
            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(alterar);

            Assert.IsTrue(exception.Message.Contains("A conta informada é inválida."));
        }

        [TestMethod]
        public async Task Alterar_ContaValidaComTodasInformacoesAlteradas_DeveAlterarTudo()
        {
            // Arrange
            var conta = new Conta
            {
                Id = 1,
                Agencia = 1,
                Numero = 1,
                Digito = 1
            };
            
            var contaAlterada = new Conta 
            {
                Id = 1,
                Agencia = 2,
                Numero = 2,
                Digito = 2
            };

            var contaRepository = Substitute.For<IContaRepository>();

            contaRepository.Any(Arg.Any<int>())
                           .Returns(true);

            contaRepository.Get(contaAlterada.Id)
                           .Returns(conta);

            var pessoaRepository = Substitute.For<IPessoaRepository>();

            var contaService = new ContaService(contaRepository, pessoaRepository);

            // Act
            await contaService.Alterar(contaAlterada);

            // Assert
            Assert.AreEqual(contaAlterada.Agencia, conta.Agencia);
            Assert.AreEqual(contaAlterada.Numero, conta.Numero);
            Assert.AreEqual(contaAlterada.Digito, conta.Digito);
        }

        [TestMethod]
        public async Task Alterar_ContaValidaComAgenciaAlterada_DeveAlterarApenasAgencia()
        {
            // Arrange
            var conta = new Conta
            {
                Id = 1,
                Agencia = 1,
                Numero = 1,
                Digito = 1
            };

            var contaAlterada = new Conta
            {
                Id = 1,
                Agencia = 2,
            };

            var contaRepository = Substitute.For<IContaRepository>();

            contaRepository.Any(Arg.Any<int>())
                           .Returns(true);

            contaRepository.Get(contaAlterada.Id)
                           .Returns(conta);

            var pessoaRepository = Substitute.For<IPessoaRepository>();

            var contaService = new ContaService(contaRepository, pessoaRepository);

            // Act
            await contaService.Alterar(contaAlterada);

            // Assert
            Assert.AreEqual(contaAlterada.Agencia, conta.Agencia);
            Assert.AreEqual(1, conta.Numero);
            Assert.AreEqual(1, conta.Digito);
        }

        [TestMethod]
        public async Task Alterar_ContaValidaComNumeroAlterado_DeveAlterarApenasNumero()
        {
            // Arrange
            var conta = new Conta
            {
                Id = 1,
                Agencia = 1,
                Numero = 1,
                Digito = 1
            };

            var contaAlterada = new Conta
            {
                Id = 1,
                Numero = 2,
            };

            var contaRepository = Substitute.For<IContaRepository>();

            contaRepository.Any(Arg.Any<int>())
                           .Returns(true);

            contaRepository.Get(contaAlterada.Id)
                           .Returns(conta);

            var pessoaRepository = Substitute.For<IPessoaRepository>();

            var contaService = new ContaService(contaRepository, pessoaRepository);

            // Act
            await contaService.Alterar(contaAlterada);

            // Assert
            Assert.AreEqual(1, conta.Agencia);
            Assert.AreEqual(contaAlterada.Numero, conta.Numero);
            Assert.AreEqual(1, conta.Digito);
        }

        [TestMethod]
        public async Task Alterar_ContaValidaComDigitoAlterado_DeveAlterarApenasDigito()
        {
            // Arrange
            var conta = new Conta
            {
                Id = 1,
                Agencia = 1,
                Numero = 1,
                Digito = 1
            };

            var contaAlterada = new Conta
            {
                Id = 1,
                Digito = 2
            };

            var contaRepository = Substitute.For<IContaRepository>();

            contaRepository.Any(Arg.Any<int>())
                           .Returns(true);

            contaRepository.Get(contaAlterada.Id)
                           .Returns(conta);

            var pessoaRepository = Substitute.For<IPessoaRepository>();

            var contaService = new ContaService(contaRepository, pessoaRepository);

            // Act
            await contaService.Alterar(contaAlterada);

            // Assert
            Assert.AreEqual(1, conta.Agencia);
            Assert.AreEqual(1, conta.Numero);
            Assert.AreEqual(contaAlterada.Digito, conta.Digito);
        }

        [TestMethod]
        public async Task Remover_IdInvalido_DeveLancarExcecaoContaInvalida()
        {
            // Arrange
            var conta1 = new Conta { Id = 1 };
            var conta2 = new Conta { Id = 2 };

            var queryContas = new List<Conta>
            {
                conta1
            }.AsQueryable();

            Expression<Func<Conta, bool>> anyIdExpression(int id) => x => x.Id == id;

            var contaRepository = Substitute.For<IContaRepository>();

            contaRepository.Any(Arg.Any<int>())
                           .Returns(args => queryContas.Any(anyIdExpression((int)args[0])));

            var pessoaRepository = Substitute.For<IPessoaRepository>();

            var contaService = new ContaService(contaRepository, pessoaRepository);

            // Act
            Func<Task> remover = () => contaService.Remover(conta2.Id);

            // Assert
            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(remover);

            Assert.IsTrue(exception.Message.Contains("A conta informada é inválida."));
        }

        [TestMethod]
        public async Task Remover_IdValido_DeveRemover()
        {
            // Arrange
            var conta1 = new Conta { Id = 1 };
            var conta2 = new Conta { Id = 2 };

            var contas = new List<Conta>
            {
                conta1,
                conta2
            };

            var queryContas = contas.AsQueryable();

            Func<Conta, bool> sameIdFunc(int id) => x => x.Id == id;

            var contaRepository = Substitute.For<IContaRepository>();

            contaRepository.Any(Arg.Any<int>())
                           .Returns(true);

            contaRepository.Get(Arg.Any<int>())
                           .Returns(args => contas.Single(sameIdFunc((int)args[0])));

            contaRepository.When(x => x.DeleteAsync(Arg.Any<Conta>()))
                           .Do(args => contas.Remove((Conta)args[0]));

            var pessoaRepository = Substitute.For<IPessoaRepository>();

            var contaService = new ContaService(contaRepository, pessoaRepository);

            // Act
            await contaService.Remover(conta2.Id);

            // Assert
            Assert.AreEqual(1, queryContas.Count());
            Assert.AreSame(conta1, contas[0]);
        }
    }
}
