using FastEndpoints;
using Webb_Labb02_version2_ApiAndBlazor.Api.Models.ResponseDto;
using Webb_Labb02_version2_ApiAndBlazor.Api.Repositories.Interfaces;

namespace Webb_Labb02_version2_ApiAndBlazor.Api.Endpoints.Orders
{
    public class GetOrderByIdEndpoint : EndpointWithoutRequest<OrderResponse>
    {
        private readonly IUnitOfWork _uow;

        public GetOrderByIdEndpoint(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public override void Configure()
        {
            Get("/orders/{id}");
            Roles("admin");

            Summary(s =>
            {
                s.Summary = "Hämtar en order med ID";
                s.Description = "Endast admin kan hämta en specifik order.";
                s.Params["id"] = "Order-ID som ska hämtas";
                s.Response<OrderResponse>(200, "Order hittades");
                s.Response(404, "Order hittades inte");
            });
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var id = Route<int>("id");

            var order = await _uow.Orders.GetByIdAsync(id);
            if (order is null)
            {
                await SendNotFoundAsync(ct);
                return;
            }

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

            await SendAsync(response, cancellation: ct);
        }
    }
}
