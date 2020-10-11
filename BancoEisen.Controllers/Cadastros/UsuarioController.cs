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
    public class UsuarioController : IUsuarioController
    {
        private readonly IUsuarioRepositorio usuarioRepositorio;

        public UsuarioController(IUsuarioRepositorio usuarioRepositorio)
        {
            this.usuarioRepositorio = usuarioRepositorio;
        }

        public Usuario[] Todos(UsuarioFiltro filtro = null, Ordem ordenacao = null)
        {
            var query = usuarioRepositorio.All();
            query = usuarioRepositorio.Filtrar(query, filtro);
            query = usuarioRepositorio.Ordenar(query, ordenacao);

            return query.ToArray();
        }

        public Usuario Consultar(int usuarioId)
        {
            return usuarioRepositorio.Get(usuarioId);
        }

        public async Task<Usuario> Cadastrar(UsuarioInformacoes usuarioInformacoes)
        {
            if (!EstaDisponivel(usuarioInformacoes.Login))
                throw new InvalidOperationException("O login informado já está em uso.");

            var usuario = new Usuario(usuarioInformacoes.Login, usuarioInformacoes.Senha);

            await usuarioRepositorio.PostAsync(usuario);

            return usuario;
        }

        public async Task Alterar(Usuario usuario)
        {
            if (!usuarioRepositorio.Any(usuario.Id))
                throw new ArgumentException("O usuário informado é inválido.");

            await usuarioRepositorio.UpdateAsync(usuario);
        }

        public async Task Remover(int usuarioId)
        {
            if (!usuarioRepositorio.Any(usuarioId))
                throw new ArgumentException("O usuário informado é inválido.");

            var usuario = usuarioRepositorio.Get(usuarioId);

            await usuarioRepositorio.DeleteAsync(usuario);
        }

        public bool EstaDisponivel(string usuarioLogin)
        {
            return usuarioRepositorio.EstaDisponivel(usuarioLogin);
        }
    }
}
