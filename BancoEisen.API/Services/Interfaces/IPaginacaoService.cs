using BancoEisen.API.Models;

namespace BancoEisen.API.Services
{
    public interface IPaginacaoService
    {
        Pagina GerarPagina(string controllerName, object[] entidades, Paginacao paginacao);
    }
}
