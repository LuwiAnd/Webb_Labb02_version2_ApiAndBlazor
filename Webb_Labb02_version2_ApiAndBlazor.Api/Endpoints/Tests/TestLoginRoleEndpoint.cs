using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Webb_Labb02_version2_ApiAndBlazor.Api.Endpoints.Tests
{
    [Authorize]
    public class TestLoginRoleEndpoint : EndpointWithoutRequest<string>
    {
        public override void Configure()
        {
            Get("/test/role");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrWhiteSpace(role))
            {
                role = "Ingen roll hittades.";
            }

            await SendAsync($"Du är inloggad som: {role}.", cancellation: ct);
        }
    }
}
