using BancoEisen.API.Models.Paginacoes;
using BancoEisen.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace BancoEisen.API.Services
{
    public class PaginacaoService : IPaginacaoService
    {
        public Pagina GerarPagina(string controllerName, object[] entidades, Paginacao paginacao)
        {
            var totalItens = entidades.Length;
            var totalPaginas = (int)Math.Ceiling(totalItens / (double)paginacao.Quantidade);

            return new Pagina(totalItens: totalItens,
                              totalPaginas: totalPaginas,
                              numeroPagina: paginacao.Pagina,
                              quantidadeItens: paginacao.Quantidade,
                              resultado: entidades.Skip(paginacao.Quantidade * (paginacao.Pagina - 1)).Take(paginacao.Quantidade).ToArray(),
                              anterior: GerarPaginaAnterior(controllerName, paginacao.Pagina, paginacao.Quantidade),
                              proxima: GerarProximaPagina(controllerName, paginacao.Pagina, paginacao.Quantidade, totalPaginas));
        }

        private string GerarPaginaAnterior(string controller, int pagina, int quantidade)
        {
            return (pagina > 1) ? GerarResourcePagina(controller, pagina - 1, quantidade) : string.Empty;
        }

        private string GerarProximaPagina(string controller, int pagina, int quantidade, int totalPaginas)
        {
            return (pagina < totalPaginas) ? GerarResourcePagina(controller, pagina + 1, quantidade) : string.Empty;
        }

        private string GerarResourcePagina(string controller, int pagina, int quantidade)
        {
            return $"{controller}?pagina={pagina}&quantidade={quantidade}";
        }
    }
}
