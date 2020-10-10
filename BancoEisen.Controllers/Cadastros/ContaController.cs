using BancoEisen.Controllers.Interfaces;
using BancoEisen.Data;
using BancoEisen.Models.Cadastros;
using BancoEisen.Models.Informacoes;
using System;

namespace BancoEisen.Controllers.Cadastros
{
    public class ContaController : IContaController
    {
        private readonly IRepository<Conta> contaRepository;
        private readonly IRepository<Pessoa> pessoaRepository;

        public ContaController(IRepository<Conta> contaRepository, IRepository<Pessoa> pessoaRepository)
        {
            this.contaRepository = contaRepository;
            this.pessoaRepository = pessoaRepository;
        }

        public Conta Cadastrar(ContaInformacoes informacoes)
        {
            if (!pessoaRepository.Any(informacoes.TitularId))
                throw new ArgumentException("O titular informado é inválido.");

            var conta = new Conta(informacoes.Agencia,
                                  informacoes.Numero,
                                  informacoes.Digito,
                                  informacoes.TitularId);

            return contaRepository.Post(conta);
        }

        public Conta Consultar(int id)
        {
            return contaRepository.Get(id);
        }

        public void Alterar(Conta conta)
        {
            if (!contaRepository.Any(conta.Id))
                throw new ArgumentException("A pessoa informada é inválida.");

            var contaSalva = contaRepository.Get(conta.Id);

            contaSalva.Agencia = conta.Agencia;
            contaSalva.Numero = conta.Numero;
            contaSalva.Digito = conta.Digito;

            contaRepository.Update(contaSalva);
        }

        public void Remover(int id)
        {
            if (!contaRepository.Any(id))
                throw new ArgumentException("A conta informada é inválida.");

            var conta = contaRepository.Get(id);

            contaRepository.Delete(conta);
        }

        public Conta[] Todos()
        {
            return contaRepository.All();
        }
    }
}
