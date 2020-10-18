using BancoEisen.Data.Models;
using BancoEisen.Data.Repositories;
using BancoEisen.Models.Cadastros;
using BancoEisen.Models.Informacoes;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BancoEisen.Services
{
    public class PessoaService : IPessoaService
    {
        private readonly IPessoaRepository pessoaRepository;

        public PessoaService(IPessoaRepository pessoaRepository)
        {
            this.pessoaRepository = pessoaRepository;
        }

        public Pessoa[] Todos(PessoaFiltro filtro = null, Ordem ordem = null)
        {
            var query = pessoaRepository.All();
            query = pessoaRepository.Filtrar(query, filtro);
            query = pessoaRepository.Ordenar(query, ordem);

            return query.ToArray();
        }

        public Pessoa Consultar(int id)
        {
            return pessoaRepository.Get(id);
        }

        public async Task<Pessoa> Cadastrar(PessoaInformacoes informacoes)
        {
            ValidarInformacoes(informacoes);

            var pessoa = new Pessoa(informacoes.Nome,
                                    informacoes.Sobrenome,
                                    informacoes.Cpf,
                                    informacoes.DataNascimento,
                                    informacoes.Email);

            await pessoaRepository.PostAsync(pessoa);

            return pessoa;
        }

        public async Task Alterar(Pessoa pessoa)
        {
            ValidarPessoa(pessoa);

            var pessoaSalva = pessoaRepository.Get(pessoa.Id);

            pessoaSalva.Nome = pessoa.Nome;
            pessoaSalva.Sobrenome = pessoa.Sobrenome;
            pessoaSalva.Cpf = pessoa.Cpf;
            pessoaSalva.DataNascimento = pessoa.DataNascimento;
            pessoaSalva.Email = pessoa.Email;

            await pessoaRepository.UpdateAsync(pessoaSalva);
        }

        public async Task Remover(int id)
        {
            ValidarId(id);

            var pessoa = pessoaRepository.Get(id);

            await pessoaRepository.DeleteAsync(pessoa);
        }

        private void ValidarInformacoes(PessoaInformacoes informacoes)
        {
            if (informacoes.DataNascimento > DateTime.Now)
                throw new ArgumentException("A data de nascimento não pode ser posterior a hoje.");
        }

        private void ValidarPessoa(Pessoa pessoa)
        {
            if (pessoa == null)
                throw new ArgumentException("A pessoa informada é inválida.");

            ValidarId(pessoa.Id);
        }

        private void ValidarId(int id)
        {
            if (!pessoaRepository.Any(id))
                throw new ArgumentException("A pessoa informada é inválida.");
        }
    }
}
