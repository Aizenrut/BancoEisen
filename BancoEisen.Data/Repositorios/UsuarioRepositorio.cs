using BancoEisen.Data.Contextos;
using BancoEisen.Data.Models.Filtros;
using BancoEisen.Data.Repositorios.Interfaces;
using BancoEisen.Data.Services.Interfaces;
using BancoEisen.Models.Cadastros;
using System.Linq;

namespace BancoEisen.Data.Repositorios
{
    public class UsuarioRepositorio : Repositorio<Usuario, UsuarioFiltro>, IUsuarioRepositorio
    {
        public UsuarioRepositorio(BancoEisenContext context, IFiltragemService<Usuario, UsuarioFiltro> filtragemService, IOrdenacaoService<Usuario> ordenacaoService)
            : base(context, filtragemService, ordenacaoService)
        {
        }

        public bool EstaDisponivel(string login)
        {
            return !context.Usuarios.Any(x => x.Login.Equals(login));
        }
    }
}
