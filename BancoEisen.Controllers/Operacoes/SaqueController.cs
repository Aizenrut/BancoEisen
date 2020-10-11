using BancoEisen.Controllers.Interfaces;
using BancoEisen.Models.Informacoes;
using BancoEisen.Models.Abstracoes;
using BancoEisen.Models.Operacoes;
using System;
using BancoEisen.Data.Repositorios.Interfaces;
using System.Threading.Tasks;
using BancoEisen.Data.Models.Filtros;
using BancoEisen.Data.Models.Ordens;
using System.Linq;
using BancoEisen.Data.Services.Interfaces;

namespace BancoEisen.Controllers.Operacoes
{
    public class SaqueController : ISaqueController
    {
        private readonly IContaRepositorio contaRepository;
        private readonly IOperacaoRepositorio operacaoRepository;
        private readonly IFiltragemService<Operacao, SaqueFiltro> filtragemService;
        private readonly IOrdenacaoService<Operacao> ordenacaoService;

        public SaqueController(IContaRepositorio contaRepository,
                               IOperacaoRepositorio operacaoRepository,
                               IFiltragemService<Operacao, SaqueFiltro> filtragemService,
                               IOrdenacaoService<Operacao> ordenacaoService)
        {
            this.contaRepository = contaRepository;
            this.operacaoRepository = operacaoRepository;
            this.filtragemService = filtragemService;
            this.ordenacaoService = ordenacaoService;
        }

        public Operacao[] Todos(SaqueFiltro filtro, Ordem ordem)
        {
            var query = operacaoRepository.All();
            query = filtragemService.Filtrar(query, filtro);
            query = ordenacaoService.Ordenar(query, ordem);

            return query.ToArray();
        }

        public Operacao Consultar(int saqueId)
        {
            return operacaoRepository.Get(saqueId);
        }

        public async Task<Operacao> Efetivar(OperacaoUnariaInformacoes saqueInformacoes)
        {
            if (saqueInformacoes.Valor <= 0)
                throw new ArgumentException("O valor a sacar deve ser maior que zero.");

            if (!contaRepository.Any(saqueInformacoes.ContaId))
                throw new ArgumentException($"A conta informada é inválida.");

            var conta = contaRepository.Get(saqueInformacoes.ContaId);

            if (conta.Saldo < saqueInformacoes.Valor)
                throw new InvalidOperationException("O saldo da conta é insuficiente para realizar a operação.");

            var saque = new Saque(saqueInformacoes.Valor, saqueInformacoes.Observacao);
            await operacaoRepository.PostAsync(saque);

            conta.Saldo -= saqueInformacoes.Valor;
            conta.Operacoes.Add(saque);
            await contaRepository.UpdateAsync(conta);

            return saque;
        }
    }
}
