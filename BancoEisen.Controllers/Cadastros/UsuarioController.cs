using BancoEisen.Controllers.Interfaces;
using BancoEisen.Models.Informacoes;
using BancoEisen.Models.Cadastros;
using BancoEisen.Data;
using System;

namespace BancoEisen.Controllers.Cadastros
{
    public class UsuarioController : IUsuarioController
    {
        private readonly IRepository<Usuario> usuarioRepository;

        public UsuarioController(IRepository<Usuario> usuarioRepository)
        {
            this.usuarioRepository = usuarioRepository;
        }

        public void Alterar(Usuario usuario)
        {
            if (!usuarioRepository.Any(usuario.Id))
                throw new ArgumentException("O usuário informado é inválido.");

            usuarioRepository.Update(usuario);
        }

        public Usuario Cadastrar(UsuarioInformacoes usuarioInformacoes)
        {
            if (!EstaDisponivel(usuarioInformacoes.Login))
                throw new InvalidOperationException("O login informado já está em uso.");

            var usuario = new Usuario(usuarioInformacoes.Login, usuarioInformacoes.Senha);

            return usuarioRepository.Post(usuario);
        }

        public Usuario Consultar(int usuarioId)
        {
            return usuarioRepository.Get(usuarioId);
        }

        public bool EstaDisponivel(string usuarioLogin)
        {
            return !usuarioRepository.Any(x => x.Login == usuarioLogin);
        }

        public void Remover(int usuarioId)
        {
            if (!usuarioRepository.Any(usuarioId))
                throw new ArgumentException("O usuário informado é inválido.");

            var usuario = usuarioRepository.Get(usuarioId);

            usuarioRepository.Delete(usuario);
        }

        public Usuario[] Todos()
        {
            return usuarioRepository.All();
        }
    }
}
