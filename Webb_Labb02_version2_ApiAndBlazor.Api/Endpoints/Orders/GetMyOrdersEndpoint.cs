using FastEndpoints;
using FastEndpoints.Security;
using System.Security.Claims;
using Webb_Labb02_version2_ApiAndBlazor.Api.Models.ResponseDto;
using Webb_Labb02_version2_ApiAndBlazor.Api.Repositories.Interfaces;



namespace Webb_Labb02_version2_ApiAndBlazor.Api.Endpoints.Orders
{
    public class GetMyOrdersEndpoint : EndpointWithoutRequest<IEnumerable<OrderResponse>>
    {
        private readonly IUnitOfWork _uow;

        public GetMyOrdersEndpoint(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public override void Configure()
        {
            Get("/orders/mine");
            Roles("user", "admin");
            Summary(s =>
            {
                s.Summary = "Hämtar ordrar för den inloggade användaren";
                s.Description = "Returnerar en lista med alla ordrar kopplade till den inloggade användarens ID.";
                s.Response<IEnumerable<OrderResponse>>(200, "Ordrar hittades");
                s.Response(401, "Ej inloggad");
            });
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            //Testkod:
            //Console.WriteLine("HandleAsync for GetMyOrdersEndpoint was called!");
            //foreach (var claim in User.Claims)
            //{
            //    Console.WriteLine($"CLAIM: {claim.Type} = {claim.Value}");
            //}
            //var userIdStr = User.ClaimValue("sub");
            //Console.WriteLine(userIdStr);

            var userIdStr = User.ClaimValue(ClaimTypes.NameIdentifier);


            if (!int.TryParse(userIdStr, out var userId))
            {
                await SendUnauthorizedAsync(ct);
                return;
            }

            var orders = await _uow.Orders.GetByUserIdAsync(userId);

            var response = orders.Select(order => new OrderResponse
            {
                OrderID = order.OrderID,
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
            });

            await SendAsync(response, cancellation: ct);
        }
    }
}
