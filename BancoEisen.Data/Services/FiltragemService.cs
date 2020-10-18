using BancoEisen.Data.Models;
using BancoEisen.Data.Services.Interfaces;
using BancoEisen.Models.Abstracoes;
using BancoEisen.Models.Enumeracoes;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace BancoEisen.Data.Services
{
    public class FiltragemService<TEntidade, TFiltro> : IFiltragemService<TEntidade, TFiltro>
        where TEntidade : Entidade
        where TFiltro : IFiltro<TEntidade>
    {
        public IQueryable<TEntidade> Filtrar(IQueryable<TEntidade> query, TFiltro filtro)
        {
            if (filtro != null)
            {
                var filtrosAplicados = filtro.GetType().GetProperties().Select(x => new
                {
                    Tipo = x.PropertyType,
                    Nome = x.Name,
                    Valor = x.GetValue(filtro)
                });

                foreach (var campo in filtrosAplicados)
                {
                    if (campo.Tipo == typeof(string))
                        query = FiltrarCampoString(query, campo.Nome, (string)campo.Valor);

                    else if (campo.Tipo == typeof(int))
                        query = FiltrarCampoInt(query, campo.Nome, (int)campo.Valor);

                    else if (campo.Tipo == typeof(DateTime))
                        query = FiltrarCampoDateTime(query, campo.Nome, (DateTime)campo.Valor);

                    else if (campo.Tipo == typeof(TipoOperacao))
                        query = FiltrarCampoTipoOperacao(query, campo.Nome, (TipoOperacao)campo.Valor);
                }
            }

            return query;
        }

        private IQueryable<TEntidade> FiltrarCampoString(IQueryable<TEntidade> query, string campo, string filtro)
        {
            if (filtro != null)
            {
                query = query.Where($"{campo}.Contains(\"{filtro}\")");
            }

            return query;
        }

        private IQueryable<TEntidade> FiltrarCampoInt(IQueryable<TEntidade> query, string campo, int filtro)
        {
            if (filtro > 0)
            {
                query = query.Where($"{campo} == {filtro}");
            }

            return query;
        }

        private IQueryable<TEntidade> FiltrarCampoDateTime(IQueryable<TEntidade> query, string campo, DateTime filtro)
        {
            if (filtro > default(DateTime))
            {
                if (filtro.Year > 0)
                {
                    query = query.Where($"{campo}.Year == {filtro.Year}");
                }

                if (filtro.Month > 0)
                {
                    query = query.Where($"{campo}.Month == {filtro.Month}");
                }

                if (filtro.Day > 0)
                {
                    query = query.Where($"{campo}.Day == {filtro.Day}");
                }
            }

            return query;
        }

        private IQueryable<TEntidade> FiltrarCampoTipoOperacao(IQueryable<TEntidade> query, string campo, TipoOperacao filtro)
        {
            return query.Where($"{campo} == {(int)filtro}");
        }
    }
}
