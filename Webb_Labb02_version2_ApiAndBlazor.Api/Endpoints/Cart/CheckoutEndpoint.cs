using FastEndpoints;
using FastEndpoints.Security;
using System.Security.Claims;
using Webb_Labb02_version2_ApiAndBlazor.Api.Entities;
using Webb_Labb02_version2_ApiAndBlazor.Api.Repositories.Interfaces;

namespace Webb_Labb02_version2_ApiAndBlazor.Api.Endpoints.Cart
{
    public class CheckoutEndpoint : EndpointWithoutRequest
    {
        private readonly IUnitOfWork _uow;

        public CheckoutEndpoint(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public override void Configure()
        {
            Post("/cart/checkout");
            Roles("user", "admin");

            Summary(s =>
            {
                s.Summary = "Genomför en beställning";
                s.Description = "Bekräftar ordern, minskar lagersaldo och tömmer kundvagnen.";
                s.Response(200, "Beställningen genomfördes");
                s.Response(400, "Fel, t.ex. otillräckligt lager");
                s.Response(401, "Ej inloggad");
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
            if (order is null || order.OrderItems.Count == 0)
            {
                await SendErrorsAsync(400, ct);
                return;
            }

            foreach (var item in order.OrderItems)
            {
                if (item.Product is null)
                {
                    item.Product = await _uow.Products.GetByIdAsync(item.ProductID);
                    if (item.Product is null)
                    {
                        AddError(o => o, $"Produkt med ID {item.ProductID} hittades inte.");
                        await SendErrorsAsync(400, ct);
                        return;
                    }
                }

                if (item.Quantity > item.Product.StockQuantity)
                {
                    AddError(o => o, $"Det finns inte tillräckligt i lager för {item.Product.Name}.");
                    await SendErrorsAsync(400, ct);
                    return;
                }

                item.Product.StockQuantity -= item.Quantity;
            }

            order.OrderStatus = "confirmed";
            order.OrderDate = DateTime.UtcNow;

            await _uow.CompleteAsync();

            await SendOkAsync(ct);
        }
    }
}
