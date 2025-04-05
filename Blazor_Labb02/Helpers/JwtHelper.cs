using System.IdentityModel.Tokens.Jwt;

namespace Blazor_Labb02.Helpers
{
    public static class JwtHelper
    {
        public static string? GetRoleFromToken(string jwt)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);
            return token.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
        }

        public static string? GetEmailFromToken(string jwt)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);
            return token.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
        }
    }
}
