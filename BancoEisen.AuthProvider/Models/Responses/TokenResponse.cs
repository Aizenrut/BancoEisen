namespace BancoEisen.AuthProvider.Models
{
    public class TokenResponse
    {
        public string Token { get; set; }

        public TokenResponse()
        {

        }

        public TokenResponse(string token)
        {
            Token = token;
        }
    }
}
