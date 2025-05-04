using FastEndpoints;
using FastEndpoints.Security;
using System.Security.Claims;
using Webb_Labb02_version2_ApiAndBlazor.Api.Models.ResponseDto;
using Webb_Labb02_version2_ApiAndBlazor.Api.Repositories.Interfaces;

namespace Webb_Labb02_version2_ApiAndBlazor.Api.Endpoints.Orders
{
    public class GetCartEndpoint : EndpointWithoutRequest<List<CartItemResponse>>
    {
        private readonly IUnitOfWork _uow;

        public GetCartEndpoint(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public override void Configure()
        {
            Get("/cart");
            Roles("user", "admin");

            Summary(s =>
            {
                s.Summary = "Hämtar den aktuella kundvagnen";
                s.Description = "Returnerar alla produkter i den pågående obekräftade ordern.";
                s.Response(200, "Lista över produkter i kundvagn");
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
                await SendAsync(new List<CartItemResponse>(), cancellation: ct);
                return;
            }

            var response = order.OrderItems.Select(i => new CartItemResponse
            {
                ProductID = i.ProductID,
                ProductName = i.Product?.Name ?? "",
                Price = i.Price,
                Quantity = i.Quantity,
                StockQuantity = i.Product?.StockQuantity ?? 0
            }).ToList();

            await SendAsync(response, cancellation: ct);
        }
    }
}
