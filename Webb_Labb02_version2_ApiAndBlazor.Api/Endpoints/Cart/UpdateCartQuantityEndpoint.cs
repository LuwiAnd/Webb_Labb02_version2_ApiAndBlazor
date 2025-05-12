using FastEndpoints;
using FastEndpoints.Security;
using System.Security.Claims;
using Webb_Labb02_version2_ApiAndBlazor.Api.Models.RequestDto;
using Webb_Labb02_version2_ApiAndBlazor.Api.Repositories.Interfaces;

namespace Webb_Labb02_version2_ApiAndBlazor.Api.Endpoints.Cart
{
    public class UpdateCartQuantityEndpoint : Endpoint<AddToCartRequest>
    {
        private readonly IUnitOfWork _uow;

        public UpdateCartQuantityEndpoint(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public override void Configure()
        {
            Put("/cart/update");
            Roles("user", "admin");

            Summary(s =>
            {
                s.Summary = "Uppdaterar antal för en produkt i kundvagnen";
                s.Description = "Sätter ett nytt exakt antal för en produkt i kundvagnen";
                s.Response(204, "Antal uppdaterat");
                s.Response(400, "Felaktiga uppgifter eller lagerbrist");
                s.Response(404, "Produkten eller kundvagnen hittades inte");
            });
        }

        public override async Task HandleAsync(AddToCartRequest req, CancellationToken ct)
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

            var item = order.OrderItems.FirstOrDefault(i => i.ProductID == req.ProductID);
            if (item is null)
            {
                await SendNotFoundAsync(ct);
                return;
            }

            var product = await _uow.Products.GetByIdAsync(req.ProductID);
            if (product is null || req.Quantity < 1 || req.Quantity > product.StockQuantity)
            {
                AddError(r => r.Quantity, $"Ogiltigt antal eller överskrider lagersaldo ({product?.StockQuantity ?? 0}).");
                await SendErrorsAsync(400, ct);
                return;
            }

            item.Quantity = req.Quantity;
            order.TotalAmount = order.OrderItems.Sum(i => i.Quantity * i.Price);

            await _uow.CompleteAsync();
            await SendNoContentAsync(ct);
        }
    }
}
