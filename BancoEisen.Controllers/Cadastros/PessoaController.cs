using BancoEisen.Controllers.Interfaces;
using BancoEisen.Data;
using BancoEisen.Models.Cadastros;
using BancoEisen.Models.Informacoes;
using System;

namespace BancoEisen.Controllers.Cadastros
{
    public class PessoaController : IPessoaController
    {
        private readonly IRepository<Pessoa> pessoaRepository;
        private readonly IRepository<Usuario> usuarioRepository;

        public PessoaController(IRepository<Pessoa> pessoaRepository, IRepository<Usuario> usuarioRepository)
        {
            this.pessoaRepository = pessoaRepository;
            this.usuarioRepository = usuarioRepository;
        }

        public void Alterar(Pessoa pessoa)
        {
            if (!pessoaRepository.Any(pessoa.Id))
                throw new ArgumentException("A pessoa informada é inválida");

            var pessoaSalva = pessoaRepository.Get(pessoa.Id);

            pessoaSalva.Nome = pessoa.Nome;
            pessoaSalva.Sobrenome = pessoa.Sobrenome;
            pessoaSalva.Cpf = pessoa.Cpf;
            pessoaSalva.DataNascimento = pessoa.DataNascimento;
            pessoaSalva.Email = pessoa.Email;

            pessoaRepository.Update(pessoaSalva);
        }

        public Pessoa Cadastrar(PessoaInformacoes informacoes)
        {
            if (informacoes.DataNascimento > DateTime.Now)
                throw new ArgumentException("A data de nascimento não pode ser posterior a hoje");

            if (!usuarioRepository.Any(informacoes.UsuarioId))
                throw new ArgumentException("O usuário informado é inválido");

            var pessoa = new Pessoa(informacoes.Nome,
                                    informacoes.Sobrenome,
                                    informacoes.Cpf,
                                    informacoes.DataNascimento,
                                    informacoes.Email,
                                    informacoes.UsuarioId);

            return pessoaRepository.Post(pessoa);
        }

        public Pessoa Consultar(int id)
        {
            return pessoaRepository.Get(id);
        }

        public void Remover(int id)
        {
            if (!pessoaRepository.Any(id))
                throw new ArgumentException("A pessoa informada é inválida");

            var pessoa = pessoaRepository.Get(id);

            pessoaRepository.Delete(pessoa);
        }

        public Pessoa[] Todos()
        {
            return pessoaRepository.All();
        }
    }
}
