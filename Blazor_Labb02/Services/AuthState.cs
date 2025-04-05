using System.IdentityModel.Tokens.Jwt;

namespace Blazor_Labb02.Services;

public class AuthState
{
    public string? Token { get; private set; }
    public string? Role { get; private set; }
    public string? Email { get; private set; }

    public event Action? OnChange;

    public void SetToken(string token)
    {
        Token = token;

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        Email = jwt.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
        Role = jwt.Claims.FirstOrDefault(c => c.Type == "role")?.Value;

        NotifyStateChanged();
    }

    public void Clear()
    {
        Token = null;
        Email = null;
        Role = null;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
