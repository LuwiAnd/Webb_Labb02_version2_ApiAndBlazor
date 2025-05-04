using FastEndpoints;
using FastEndpoints.Security;
using System.Security.Claims;
using Webb_Labb02_version2_ApiAndBlazor.Api.Entities;
using Webb_Labb02_version2_ApiAndBlazor.Api.Models.RequestDto;
using Webb_Labb02_version2_ApiAndBlazor.Api.Repositories.Interfaces;

namespace Webb_Labb02_version2_ApiAndBlazor.Api.Endpoints.Cart
{
    public class AddToCartEndpoint : Endpoint<AddToCartRequest>
    {
        private readonly IUnitOfWork _uow;

        public AddToCartEndpoint(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public override void Configure()
        {
            Post("/cart/add");
            Roles("user", "admin");

            Summary(s =>
            {
                s.Summary = "Lägger till en produkt i kundvagnen";
                s.Description = "Om en kundvagn inte finns skapas den. Om produkten redan finns i kundvagnen uppdateras mängden.";
                s.Response(200, "Produkt tillagd i kundvagnen");
                s.Response(400, "Felaktig data eller lagerbrist");
                s.Response(401, "Ej inloggad");
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

            var product = await _uow.Products.GetByIdAsync(req.ProductID);
            if (product is null)
            {
                AddError(r => r.ProductID, "Produkten hittades inte.");
                await SendErrorsAsync(400, ct);
                return;
            }

            if (req.Quantity <= 0)
            {
                AddError(r => r.Quantity, "Antalet måste vara större än 0.");
                await SendErrorsAsync(400, ct);
                return;
            }

            if (product.StockQuantity < req.Quantity)
            {
                AddError(r => r.Quantity, "Det finns inte tillräckligt många i lager.");
                await SendErrorsAsync(400, ct);
                return;
            }

            var order = await _uow.Orders.GetUnhandledOrderAsync(userId);
            if (order is null)
            {
                order = new Webb_Labb02_version2_ApiAndBlazor.Api.Entities.Order
                {
                    UserID = userId,
                    OrderDate = DateTime.UtcNow,
                    OrderStatus = "unhandled",
                    OrderItems = new List<OrderItem>()
                };
                await _uow.Orders.AddAsync(order);
            }

            var existingItem = order.OrderItems.FirstOrDefault(i => i.ProductID == req.ProductID);
            if (existingItem is not null)
            {
                var newQty = existingItem.Quantity + req.Quantity;
                if (newQty > product.StockQuantity)
                {
                    AddError(r => r.Quantity, $"Totala antalet överskrider lagersaldot ({product.StockQuantity}).");
                    await SendErrorsAsync(400, ct);
                    return;
                }

                existingItem.Quantity = newQty;
            }
            else
            {
                order.OrderItems.Add(new OrderItem
                {
                    ProductID = product.ID,
                    Quantity = req.Quantity,
                    Price = product.Price
                });
            }

            order.TotalAmount = order.OrderItems.Sum(i => i.Price * i.Quantity);

            await _uow.CompleteAsync();
            await SendOkAsync(ct);
        }
    }
}
