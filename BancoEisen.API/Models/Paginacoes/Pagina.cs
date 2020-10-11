namespace BancoEisen.API.Models.Paginacoes
{
    public class Pagina
    {
        public int TotalItens { get; set; }
        public int TotalPaginas { get; set; }
        public int NumeroPagina { get; set; }
        public int QuantidadeItens { get; set; }
        public object[] Resultado { get; set; }
        public string Anterior { get; set; }
        public string Proxima { get; set; }

        public Pagina()
        {

        }

        public Pagina(int totalItens, int totalPaginas, int numeroPagina, int quantidadeItens, object[] resultado, string anterior, string proxima)
        {
            TotalItens = totalItens;
            TotalPaginas = totalPaginas;
            NumeroPagina = numeroPagina;
            QuantidadeItens = quantidadeItens;
            Resultado = resultado;
            Anterior = anterior;
            Proxima = proxima;
        }
    }
}
