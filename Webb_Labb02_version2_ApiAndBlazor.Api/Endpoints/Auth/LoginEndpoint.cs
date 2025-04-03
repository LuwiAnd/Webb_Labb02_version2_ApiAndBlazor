using FastEndpoints;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Webb_Labb02_version2_ApiAndBlazor.Api.Data;
using Webb_Labb02_version2_ApiAndBlazor.Api.Models.RequestDto;
using Webb_Labb02_version2_ApiAndBlazor.Api.Models.ResponseDto;

using Microsoft.AspNetCore.Identity;
using Webb_Labb02_version2_ApiAndBlazor.Api.Entities;



namespace Webb_Labb02_version2_ApiAndBlazor.Api.Endpoints.Auth
{
    public class LoginEndpoint : Endpoint<LoginRequest, LoginResponse>
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;
        private readonly PasswordHasher<User> _hasher = new();

        public LoginEndpoint(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public override void Configure()
        {
            Post("/login");
            AllowAnonymous();

            Summary(s =>
            {
                s.Summary = "Loggar in användare och returnerar en JWT-token";
                s.Description = "Verifierar e-post och lösenord, och returnerar token vid korrekt inloggning.";
                s.Response<LoginResponse>(200, "Inloggning lyckades, JWT-token returneras");
                s.Response(401, "Ogiltig e-post eller lösenord");
            });
        }


        public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == req.Email);

            if (user == null)
            {
                await SendUnauthorizedAsync(ct);
                return;
            }


            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, req.Password);

            if(result == PasswordVerificationResult.Failed)
            {
                await SendUnauthorizedAsync(ct);
                return;
            }

            var token = GenerateJwtToken(user);

            await SendAsync(new LoginResponse
            {
                Token = token,
                Email = user.Email ?? "",
                Role = user.Role
            });
        }


        private string GenerateJwtToken(Entities.User user)
        {
            var jwtSettings = _config.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"]!;
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            var expires = DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpiresInMinutes"]!));

            //var claims = new[]
            //{
            //    new Claim(JwtRegisteredClaimNames.Sub, user.UserID.ToString()),
            //    new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
            //    new Claim(ClaimTypes.Role, user.Role),
            //};
            // Bytte ut ovanstående mot nedanstående för att slippa det långa schema-namnet.
            var claims = new[]
            {
                new Claim("sub", user.UserID.ToString()),
                new Claim("email", user.Email ?? ""),
                new Claim("role", user.Role)
            };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
