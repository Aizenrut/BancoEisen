﻿using BancoEisen.API.Controllers.Templates;
using BancoEisen.API.Services;
using BancoEisen.Data.Models;
using BancoEisen.Models.Informacoes;
using BancoEisen.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BancoEisen.API.Controllers.Operacoes
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DepositosController : OperacoesControllerTamplate<IDepositoService, OperacaoUnariaInformacoes, DepositoFiltro>
    {
        public DepositosController(IDepositoService servico, IPaginacaoService paginacaoService, IHttpContextAccessor contextAccessor) 
            : base(servico, paginacaoService, contextAccessor)
        {
        }
    }
}
