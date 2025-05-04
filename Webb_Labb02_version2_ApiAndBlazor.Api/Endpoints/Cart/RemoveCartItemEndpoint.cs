using FastEndpoints;
using FastEndpoints.Security;
using System.Security.Claims;
using Webb_Labb02_version2_ApiAndBlazor.Api.Entities;
using Webb_Labb02_version2_ApiAndBlazor.Api.Repositories.Interfaces;

namespace Webb_Labb02_version2_ApiAndBlazor.Api.Endpoints.Cart
{
    public class RemoveCartItemEndpoint : EndpointWithoutRequest
    {
        private readonly IUnitOfWork _uow;

        public RemoveCartItemEndpoint(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public override void Configure()
        {
            Delete("/cart/item/{productId}");
            Roles("user", "admin");

            Summary(s =>
            {
                s.Summary = "Tar bort en produkt från kundvagnen";
                s.Description = "Tar bort vald produkt från den aktuella obekräftade ordern.";
                s.Params["productId"] = "ID för produkten som ska tas bort";
                s.Response(204, "Produkten togs bort");
                s.Response(404, "Produkten eller kundvagnen hittades inte");
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

            var productId = Route<int>("productId");

            var order = await _uow.Orders.GetUnhandledOrderAsync(userId);
            if (order is null)
            {
                await SendNotFoundAsync(ct);
                return;
            }

            var item = order.OrderItems.FirstOrDefault(i => i.ProductID == productId);
            if (item is null)
            {
                await SendNotFoundAsync(ct);
                return;
            }

            order.OrderItems.Remove(item);
            order.TotalAmount = order.OrderItems.Sum(i => i.Price * i.Quantity);
            await _uow.CompleteAsync();

            await SendNoContentAsync(ct);
        }
    }
}
