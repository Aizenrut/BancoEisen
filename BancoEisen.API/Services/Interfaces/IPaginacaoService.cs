using BancoEisen.API.Models.Paginacoes;

namespace BancoEisen.API.Services.Interfaces
{
    public interface IPaginacaoService
    {
        Pagina GerarPagina(string controllerName, object[] entidades, Paginacao paginacao);
    }
}
