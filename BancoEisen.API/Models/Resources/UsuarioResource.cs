using BancoEisen.Models.Cadastros;

namespace BancoEisen.API.Models.Resources
{
    public struct UsuarioResource
    {
        public int Id { get; }
        public string Login { get; }
        public string Senha { get; }

        public UsuarioResource(Usuario usuario)
        {
            Id = usuario.Id;
            Login = usuario.Login;
            Senha = usuario.Senha;
        }
    }
}
