using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Blazor_Labb02.Services;

public class AuthState
{
    public string? Token { get; private set; }
    public string? Role { get; private set; }
    public string? Email { get; private set; }
    public int? UserId { get; private set; }

    public event Action? OnChange;

    public void SetToken(string token)
    {
        Token = token;

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        Email = jwt.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
        Role = jwt.Claims.FirstOrDefault(c => c.Type == "role")?.Value;

        var userIdClaim = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "sub");
        if (int.TryParse(userIdClaim?.Value, out var uid))
            UserId = uid;

        NotifyStateChanged();

        Console.WriteLine($"[SetToken] E-post: {Email}, Roll: {Role}, AnvändarID: {UserId}");

    }

    public void Clear()
    {
        Token = null;
        Email = null;
        Role = null;
        UserId = null;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
