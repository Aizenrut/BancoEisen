using BancoEisen.AuthProvider.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BancoEisen.AuthProvider.Data
{
    public class BancoEisenAuthenticationContext : IdentityDbContext<Usuario>
    {
        public BancoEisenAuthenticationContext(DbContextOptions<BancoEisenAuthenticationContext> options) : base(options)
        {
            this.Database.EnsureCreated();
        }
    }
}
