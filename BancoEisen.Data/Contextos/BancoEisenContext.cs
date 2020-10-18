using BancoEisen.Models.Abstracoes;
using BancoEisen.Models.Cadastros;
using Microsoft.EntityFrameworkCore;

namespace BancoEisen.Data.Contextos
{
    public class BancoEisenContext : DbContext
    {
        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Conta> Contas { get; set; }
        public DbSet<Operacao> Operacoes { get; set; }

        public BancoEisenContext(DbContextOptions<BancoEisenContext> options) : base(options)
        {
            this.Database.EnsureCreated();
        }
    }
}
