using BancoEisen.Controllers.Interfaces;
using BancoEisen.Data.Models.Filtros;
using BancoEisen.Data.Models.Ordens;
using BancoEisen.Data.Repositorios.Interfaces;
using BancoEisen.Models.Cadastros;
using BancoEisen.Models.Informacoes;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BancoEisen.Controllers.Cadastros
{
    public class PessoaController : IPessoaController
    {
        private readonly IPessoaRepositorio pessoaRepositorio;

        public PessoaController(IPessoaRepositorio pessoaRepositorio)
        {
            this.pessoaRepositorio = pessoaRepositorio;
        }

        public Pessoa[] Todos(PessoaFiltro filtro = null, Ordem ordem = null)
        {
            var query = pessoaRepositorio.All();
            query = pessoaRepositorio.Filtrar(query, filtro);
            query = pessoaRepositorio.Ordenar(query, ordem);

            return query.ToArray();
        }

        public Pessoa Consultar(int id)
        {
            return pessoaRepositorio.Get(id);
        }

        public async Task<Pessoa> Cadastrar(PessoaInformacoes informacoes)
        {
            if (informacoes.DataNascimento > DateTime.Now)
                throw new ArgumentException("A data de nascimento não pode ser posterior a hoje.");

            var pessoa = new Pessoa(informacoes.Nome,
                                    informacoes.Sobrenome,
                                    informacoes.Cpf,
                                    informacoes.DataNascimento,
                                    informacoes.Email);

            await pessoaRepositorio.PostAsync(pessoa);

            return pessoa;
        }

        public async Task Alterar(Pessoa pessoa)
        {
            if (!pessoaRepositorio.Any(pessoa.Id))
                throw new ArgumentException("A pessoa informada é inválida.");

            var pessoaSalva = pessoaRepositorio.Get(pessoa.Id);

            pessoaSalva.Nome = pessoa.Nome;
            pessoaSalva.Sobrenome = pessoa.Sobrenome;
            pessoaSalva.Cpf = pessoa.Cpf;
            pessoaSalva.DataNascimento = pessoa.DataNascimento;
            pessoaSalva.Email = pessoa.Email;

            await pessoaRepositorio.UpdateAsync(pessoaSalva);
        }

        public async Task Remover(int id)
        {
            if (!pessoaRepositorio.Any(id))
                throw new ArgumentException("A pessoa informada é inválida.");

            var pessoa = pessoaRepositorio.Get(id);

            await pessoaRepositorio.DeleteAsync(pessoa);
        }
    }
}
