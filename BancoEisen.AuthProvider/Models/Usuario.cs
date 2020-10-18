using Microsoft.AspNetCore.Identity;

namespace BancoEisen.AuthProvider.Models
{
    public class Usuario : IdentityUser
    {
        public Usuario() : base()
        {

        }

        public Usuario(string nomeUsuario, string email) : base(nomeUsuario)
        {
            Email = email;
        }
    }
}
