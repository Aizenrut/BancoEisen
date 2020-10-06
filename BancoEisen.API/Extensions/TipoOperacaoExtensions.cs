using BancoEisen.Models.Enumeracoes;
using System.ComponentModel;

namespace BancoEisen.API.Extensions
{
    public static class TipoOperacaoExtensions
    {
        public static string GetDescription(this TipoOperacao tipoOperacao)
        {
            var campo = tipoOperacao.GetType().GetField(tipoOperacao.ToString());
            var atributos = campo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return ((DescriptionAttribute)atributos[0]).Description;
        }
    }
}
