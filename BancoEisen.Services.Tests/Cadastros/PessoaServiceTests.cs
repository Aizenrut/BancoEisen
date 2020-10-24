using BancoEisen.Data.Repositories;
using BancoEisen.Models.Cadastros;
using BancoEisen.Models.Informacoes;
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
    public class PessoaServiceTests
    {
        [TestMethod]
        public void Todos_BuscaSemFiltroEOrdem_DeveRetornarAColecaoInicial()
        {
            // Arrange
            var pessoa1 = new Pessoa { Id = 1 };
            var pessoa2 = new Pessoa { Id = 2 };
            var pessoa3 = new Pessoa { Id = 3 };

            var queryPessoas = new List<Pessoa>
            {
                pessoa1,
                pessoa2,
                pessoa3
            }.AsQueryable();

            var pessoaRepository = Substitute.For<IPessoaRepository>();

            pessoaRepository.Filtrar(Arg.Any<IQueryable<Pessoa>>(), null)
                            .Returns(args => (IQueryable<Pessoa>)args[0]);

            pessoaRepository.Ordenar(Arg.Any<IQueryable<Pessoa>>(), null)
                            .Returns(args => (IQueryable<Pessoa>)args[0]);

            var pessoaService = new PessoaService(pessoaRepository);

            // Act
            var result = pessoaService.Todos();

            // Assert
            Assert.AreEqual(3, queryPessoas.Count());

            var listaPessoas = queryPessoas.ToList();

            Assert.AreSame(pessoa1, listaPessoas[0]);
            Assert.AreSame(pessoa2, listaPessoas[1]);
            Assert.AreSame(pessoa3, listaPessoas[2]);
        }

        [TestMethod]
        public async Task Cadastrar_DataNascimentoInvalida_DeveLancarExcecaoDataNascimentoInvalida()
        {
            // Arrange
            var pessoa = new PessoaInformacoes
            { 
                Nome = "Teste",
                Sobrenome = "Unitário",
                Cpf = "01234567890",
                DataNascimento = DateTime.Now.AddDays(1)
            };

            var pessoaRepository = Substitute.For<IPessoaRepository>();

            var pessoaService = new PessoaService(pessoaRepository);

            // Act
            Func<Task> cadastrar = () => pessoaService.Cadastrar(pessoa);

            // Assert
            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(cadastrar);

            Assert.IsTrue(exception.Message.Contains("A data de nascimento não pode ser posterior a hoje."));
        }

        [TestMethod]
        public async Task Cadastrar_InformacoesValidas_DeveCadastrarERetornarAPessoa()
        {
            // Arrange
            var pessoa = new PessoaInformacoes
            {
                Nome = "Teste",
                Sobrenome = "Unitário",
                Cpf = "01234567890",
                DataNascimento = DateTime.Now
            };

            var pessoaRepository = Substitute.For<IPessoaRepository>();

            var pessoaService = new PessoaService(pessoaRepository);

            // Act
            var result = await pessoaService.Cadastrar(pessoa);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(pessoa.Nome, result.Nome);
            Assert.AreEqual(pessoa.Sobrenome, result.Sobrenome);
            Assert.AreEqual(pessoa.Cpf, result.Cpf);
            Assert.AreEqual(pessoa.DataNascimento, result.DataNascimento);
        }

        [TestMethod]
        public void Consultar_IdValido_DeveRetornarAPessoa()
        {
            // Arrange
            var pessoa = new Pessoa { Id = 1 };

            var pessoaRepository = Substitute.For<IPessoaRepository>();

            pessoaRepository.Get(pessoa.Id)
                            .Returns(pessoa);

            var pessoaService = new PessoaService(pessoaRepository);

            // Act
            var result = pessoaService.Consultar(pessoa.Id);

            // Assert
            Assert.AreSame(pessoa, result);
        }

        [TestMethod]
        public void Consultar_IdInvalido_DeveRetornarNulo()
        {
            // Arrange
            var pessoa = new Pessoa { Id = 1 };

            var pessoaRepository = Substitute.For<IPessoaRepository>();

            pessoaRepository.Get(pessoa.Id)
                            .Returns(pessoa);

            var pessoaService = new PessoaService(pessoaRepository);

            // Act
            var result = pessoaService.Consultar(2);

            // Assert
            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public async Task Alterar_PessoaNula_DeveLancarExcecaoPessoaInvalida()
        {
            // Arrange
            var pessoaRepository = Substitute.For<IPessoaRepository>();

            var pessoaService = new PessoaService(pessoaRepository);

            // Act
            Func<Task> alterar = () => pessoaService.Alterar(null);

            // Assert
            var exception = await Assert.ThrowsExceptionAsync<ArgumentNullException>(alterar);

            Assert.IsTrue(exception.Message.Contains("A pessoa informada é inválida."));
        }

        [TestMethod]
        public async Task Alterar_IdInvalido_DeveLancarExcecaoPessoaInvalida()
        {
            // Arrange
            var pessoa1 = new Pessoa { Id = 1 };
            var pessoa2 = new Pessoa { Id = 2 };

            var queryPessoas = new List<Pessoa>
            {
                pessoa1,
            }.AsQueryable();

            Expression<Func<Pessoa, bool>> sameIdExpression(int id) => x => x.Id == id;

            var pessoaRepository = Substitute.For<IPessoaRepository>();

            pessoaRepository.Any(Arg.Any<int>())
                            .Returns(args => queryPessoas.Any(sameIdExpression((int)args[0])));

            var pessoaService = new PessoaService(pessoaRepository);

            // Act
            Func<Task> alterar = () => pessoaService.Alterar(pessoa2);

            // Assert
            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(alterar);

            Assert.IsTrue(exception.Message.Contains("A pessoa informada é inválida."));
        }

        [TestMethod]
        public async Task Alterar_PessoaValidaComTodasInformacoesAlteradas_DeveAlterarTudo()
        {
            // Arrange
            var pessoa = new Pessoa
            {
                Id = 1,
                Nome = "Teste",
                Sobrenome = "Unitário",
                Cpf = "01234567890",
                DataNascimento = DateTime.Now
            };

            var pessoaAlterada = new Pessoa
            {
                Id = 1,
                Nome = "Teste 123",
                Sobrenome = "Unitário 321",
                Cpf = "11111111111",
                DataNascimento = DateTime.Now.Subtract(TimeSpan.FromDays(365))
            };

            var pessoaRepository = Substitute.For<IPessoaRepository>();

            pessoaRepository.Any(Arg.Any<int>())
                            .Returns(true);

            pessoaRepository.Get(pessoaAlterada.Id)
                            .Returns(pessoa);

            var pessoaService = new PessoaService(pessoaRepository);

            // Act
            await pessoaService.Alterar(pessoaAlterada);

            // Assert
            Assert.AreEqual(pessoaAlterada.Nome, pessoa.Nome);
            Assert.AreEqual(pessoaAlterada.Sobrenome, pessoa.Sobrenome);
            Assert.AreEqual(pessoaAlterada.Cpf, pessoa.Cpf);
            Assert.AreEqual(pessoaAlterada.DataNascimento, pessoa.DataNascimento);
        }

        [TestMethod]
        public async Task Alterar_PessoaValidaComNomeAlterado_DeveAlterarApenasNome()
        {
            var dataNascimento = DateTime.Now;

            // Arrange
            var pessoa = new Pessoa
            {
                Id = 1,
                Nome = "Teste",
                Sobrenome = "Unitário",
                Cpf = "01234567890",
                DataNascimento = dataNascimento
            };

            var pessoaAlterada = new Pessoa
            {
                Id = 1,
                Nome = "Teste 123",
            };

            var pessoaRepository = Substitute.For<IPessoaRepository>();

            pessoaRepository.Any(Arg.Any<int>())
                            .Returns(true);

            pessoaRepository.Get(pessoaAlterada.Id)
                            .Returns(pessoa);

            var pessoaService = new PessoaService(pessoaRepository);

            // Act
            await pessoaService.Alterar(pessoaAlterada);

            // Assert
            Assert.AreEqual(pessoaAlterada.Nome, pessoa.Nome);
            Assert.AreEqual("Unitário", pessoa.Sobrenome);
            Assert.AreEqual("01234567890", pessoa.Cpf);
            Assert.AreEqual(dataNascimento, pessoa.DataNascimento);
        }

        [TestMethod]
        public async Task Alterar_PessoaValidaComSobrenomeAlterado_DeveAlterarApenasSobrenome()
        {
            var dataNascimento = DateTime.Now;

            // Arrange
            var pessoa = new Pessoa
            {
                Id = 1,
                Nome = "Teste",
                Sobrenome = "Unitário",
                Cpf = "01234567890",
                DataNascimento = dataNascimento
            };

            var pessoaAlterada = new Pessoa
            {
                Id = 1,
                Sobrenome = "Unitário 321"
            };

            var pessoaRepository = Substitute.For<IPessoaRepository>();

            pessoaRepository.Any(Arg.Any<int>())
                            .Returns(true);

            pessoaRepository.Get(pessoaAlterada.Id)
                            .Returns(pessoa);

            var pessoaService = new PessoaService(pessoaRepository);

            // Act
            await pessoaService.Alterar(pessoaAlterada);

            // Assert
            Assert.AreEqual("Teste", pessoa.Nome);
            Assert.AreEqual(pessoaAlterada.Sobrenome, pessoa.Sobrenome);
            Assert.AreEqual("01234567890", pessoa.Cpf);
            Assert.AreEqual(dataNascimento, pessoa.DataNascimento);
        }

        [TestMethod]
        public async Task Alterar_PessoaValidaComCpfAlterado_DeveAlterarApenasCpf()
        {
            var dataNascimento = DateTime.Now;

            // Arrange
            var pessoa = new Pessoa
            {
                Id = 1,
                Nome = "Teste",
                Sobrenome = "Unitário",
                Cpf = "01234567890",
                DataNascimento = dataNascimento
            };

            var pessoaAlterada = new Pessoa
            {
                Id = 1,
                Cpf = "11111111111"
            };

            var pessoaRepository = Substitute.For<IPessoaRepository>();

            pessoaRepository.Any(Arg.Any<int>())
                            .Returns(true);

            pessoaRepository.Get(pessoaAlterada.Id)
                            .Returns(pessoa);

            var pessoaService = new PessoaService(pessoaRepository);

            // Act
            await pessoaService.Alterar(pessoaAlterada);

            // Assert
            Assert.AreEqual("Teste", pessoa.Nome);
            Assert.AreEqual("Unitário", pessoa.Sobrenome);
            Assert.AreEqual(pessoaAlterada.Cpf, pessoa.Cpf);
            Assert.AreEqual(dataNascimento, pessoa.DataNascimento);
        }

        [TestMethod]
        public async Task Alterar_PessoaValidaComDataNascimentoAlterada_DeveAlterarApenasDataNascimento()
        {
            // Arrange
            var pessoa = new Pessoa
            {
                Id = 1,
                Nome = "Teste",
                Sobrenome = "Unitário",
                Cpf = "01234567890",
                DataNascimento = DateTime.Now
            };

            var pessoaAlterada = new Pessoa
            {
                Id = 1,
                DataNascimento = DateTime.Now.Subtract(TimeSpan.FromDays(365))
            };

            var pessoaRepository = Substitute.For<IPessoaRepository>();

            pessoaRepository.Any(Arg.Any<int>())
                            .Returns(true);

            pessoaRepository.Get(pessoaAlterada.Id)
                            .Returns(pessoa);

            var pessoaService = new PessoaService(pessoaRepository);

            // Act
            await pessoaService.Alterar(pessoaAlterada);

            // Assert
            Assert.AreEqual("Teste", pessoa.Nome);
            Assert.AreEqual("Unitário", pessoa.Sobrenome);
            Assert.AreEqual("01234567890", pessoa.Cpf);
            Assert.AreEqual(pessoaAlterada.DataNascimento, pessoa.DataNascimento);
        }

        [TestMethod]
        public async Task Remover_IdInvalido_DeveLancarExcecaoPessoaInvalida()
        {
            // Arrange
            var pessoa1 = new Pessoa { Id = 1 };
            var pessoa2 = new Pessoa { Id = 2 };

            var queryPessoas = new List<Pessoa>
            {
                pessoa1
            }.AsQueryable();

            Expression<Func<Pessoa, bool>> anyIdExpression(int id) => x => x.Id == id;

            var pessoaRepository = Substitute.For<IPessoaRepository>();

            pessoaRepository.Any(Arg.Any<int>())
                            .Returns(args => queryPessoas.Any(anyIdExpression((int)args[0])));

            var pessoaService = new PessoaService(pessoaRepository);

            // Act
            Func<Task> remover = () => pessoaService.Remover(pessoa2.Id);

            // Assert
            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(remover);

            Assert.IsTrue(exception.Message.Contains("A pessoa informada é inválida."));
        }

        [TestMethod]
        public async Task Remover_IdValido_DeveRemover()
        {
            // Arrange
            var pessoa1 = new Pessoa { Id = 1 };
            var pessoa2 = new Pessoa { Id = 2 };

            var pessoas = new List<Pessoa>
            {
                pessoa1,
                pessoa2
            };

            var queryPessoas = pessoas.AsQueryable();

            Func<Pessoa, bool> sameIdFunc(int id) => x => x.Id == id;

            var pessoaRepository = Substitute.For<IPessoaRepository>();

            pessoaRepository.Any(Arg.Any<int>())
                            .Returns(true);

            pessoaRepository.Get(Arg.Any<int>())
                            .Returns(args => pessoas.Single(sameIdFunc((int)args[0])));

            pessoaRepository.When(x => x.DeleteAsync(Arg.Any<Pessoa>()))
                            .Do(args => pessoas.Remove((Pessoa)args[0]));

            var pessoaService = new PessoaService(pessoaRepository);

            // Act
            await pessoaService.Remover(pessoa2.Id);

            // Assert
            Assert.AreEqual(1, queryPessoas.Count());
            Assert.AreSame(pessoa1, pessoas[0]);
        }
    }
}
