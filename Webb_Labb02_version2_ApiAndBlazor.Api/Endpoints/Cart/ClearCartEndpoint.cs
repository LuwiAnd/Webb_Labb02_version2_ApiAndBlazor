using FastEndpoints;
using FastEndpoints.Security;
using System.Security.Claims;
using Webb_Labb02_version2_ApiAndBlazor.Api.Repositories.Interfaces;

namespace Webb_Labb02_version2_ApiAndBlazor.Api.Endpoints.Cart
{
    public class ClearCartEndpoint : EndpointWithoutRequest
    {
        private readonly IUnitOfWork _uow;

        public ClearCartEndpoint(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public override void Configure()
        {
            Delete("/cart/clear");
            Roles("user", "admin");

            Summary(s =>
            {
                s.Summary = "Tömmer hela kundvagnen";
                s.Description = "Tar bort alla produkter från pågående kundvagn (unhandled order).";
                s.Response(204, "Kundvagnen tömd");
                s.Response(404, "Ingen aktiv kundvagn hittades");
            });
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var userIdStr = User.ClaimValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId))
            {
                await SendUnauthorizedAsync(ct);
                return;
            }

            var order = await _uow.Orders.GetUnhandledOrderAsync(userId);
            if (order is null)
            {
                await SendNotFoundAsync(ct);
                return;
            }

            order.OrderItems.Clear();
            order.TotalAmount = 0;
            await _uow.CompleteAsync();

            await SendNoContentAsync(ct);
        }
    }
}
