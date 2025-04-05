using FastEndpoints;
using Webb_Labb02_version2_ApiAndBlazor.Api.Entities;
using Webb_Labb02_version2_ApiAndBlazor.Api.Models.ResponseDto;
using Webb_Labb02_version2_ApiAndBlazor.Api.Repositories.Interfaces;

namespace Webb_Labb02_version2_ApiAndBlazor.Endpoints.Orders
{
    public class GetAllOrdersEndpoint : EndpointWithoutRequest<IEnumerable<OrderResponse>>
    {
        private readonly IUnitOfWork _uow;

        public GetAllOrdersEndpoint(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public override void Configure()
        {
            Get("orders");
            //Roles("admin"); 
            AllowAnonymous();
            Summary(s =>
            {
                s.Summary = "Hämtar alla ordrar";
                s.Description = "Endast admin får se samtliga ordrar.";
                s.Response(200, "Lista med ordrar");
                s.Response(401, "Ej inloggad");
                s.Response(403, "Inte behörig");
            });
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            //var orders = await _uow.Orders.GetAllAsync(includeItems: true);
            var orders = await _uow.Orders.GetAllAsync();

            var response = orders.Select(order => new OrderResponse
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
            }).ToList();

            await SendAsync(response, cancellation: ct);
        }

    }
}
