using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace Webb_Labb02_version2_ApiAndBlazor.Api.Endpoints.Tests
{
    [Authorize(Roles = "admin")]
    public class TestAdminEndpoint : EndpointWithoutRequest<string>
    {
        public override void Configure()
        {
            Get("/admin/test");
            //AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            await SendAsync("Du är admin!", cancellation: ct);
        }
    }
}
