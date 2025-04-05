using FastEndpoints;
using Webb_Labb02_version2_ApiAndBlazor.Api.Models.RequestDto;
using Webb_Labb02_version2_ApiAndBlazor.Api.Models.ResponseDto;
using Webb_Labb02_version2_ApiAndBlazor.Api.Repositories.Interfaces;
using Webb_Labb02_version2_ApiAndBlazor.Api.Entities;

namespace Webb_Labb02_version2_ApiAndBlazor.Api.Endpoints.Orders
{
    
    public class CreateOrderEndpoint : Endpoint<CreateOrderRequest>
    {
        private readonly IUnitOfWork _uow;

        public CreateOrderEndpoint(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public override void Configure()
        {
            Post("orders");
            Roles("user", "admin");

            Version(1);

            Summary(s =>
            {
                s.Summary = "Skapa en ny order";
                s.Description = "Registrerar en ny order för användaren med angivna produkter.";
                s.Response(201, "Order skapad");
                s.Response(400, "Felaktig data");
                s.Response(401, "Ej inloggad");
            });
        }

        public override async Task HandleAsync(CreateOrderRequest req, CancellationToken ct)
        {
            var user = await _uow.Users.GetByIdAsync(req.UserID);
            if (user is null)
            {
                await SendNotFoundAsync(ct);
                return;
            }

            var orderItems = new List<OrderItem>();
            decimal totalAmount = 0;

            foreach (OrderItemRequest item in req.Items)
            {
                var product = await _uow.Products.GetByIdAsync(item.ProductID);
                if (product is null)
                {
                    continue;
                }

                var price = product.Price;
                var orderItem = new OrderItem
                {
                    ProductID = product.ID,
                    Quantity = item.Quantity,
                    Price = price
                };

                totalAmount += price * item.Quantity;
                orderItems.Add(orderItem);
            }

            //var order = new Order <- ger namnkonflikt med FastEndpoints.Order.
            var order = new Webb_Labb02_version2_ApiAndBlazor.Api.Entities.Order
            {
                UserID = req.UserID,
                OrderDate = DateTime.UtcNow,
                OrderStatus = "pending",
                TotalAmount = totalAmount,
                OrderItems = orderItems
            };

            await _uow.Orders.AddAsync(order);
            await _uow.CompleteAsync();

            //await SendCreatedAtAsync($"/orders/{order.OrderID}", order, ct);

            var response = new OrderResponse
            {
                OrderID = order.ID,
                UserID = order.UserID,
                OrderDate = order.OrderDate,
                OrderStatus = order.OrderStatus,
                TotalAmount = order.TotalAmount,
                Items = order.OrderItems.Select(i => new OrderItemResponse
                {
                    ProductID = i.ProductID,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            };

            await SendCreatedAtAsync($"/orders/{order.ID}", response, ct);

        }
    }

}
