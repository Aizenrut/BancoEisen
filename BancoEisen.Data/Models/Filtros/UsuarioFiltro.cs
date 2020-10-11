using BancoEisen.Data.Models.Filtros.Interfaces;
using BancoEisen.Models.Cadastros;

namespace BancoEisen.Data.Models.Filtros
{
    public class UsuarioFiltro : IFiltro<Usuario>
    {
        public string Login { get; set; }
    }
}
