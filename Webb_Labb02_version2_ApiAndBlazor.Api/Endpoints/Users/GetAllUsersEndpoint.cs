using FastEndpoints;
using Webb_Labb02_version2_ApiAndBlazor.Api.Entities;
using Webb_Labb02_version2_ApiAndBlazor.Api.Repositories.Interfaces;

namespace Webb_Labb02_version2_ApiAndBlazor.Endpoints.Users
{
    public class GetAllUsersEndpoint : EndpointWithoutRequest<IEnumerable<User>>
    {
        private readonly IUnitOfWork _uow;

        public GetAllUsersEndpoint(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var users = await _uow.Users.GetAllAsync();
            await SendAsync(users);
        }

        public override void Configure()
        {
            Get("/users");
            Roles("admin");

            Summary(s =>
            {
                s.Summary = "Hämtar alla användare (endast för admin)";
                s.Description = "Returnerar en lista av alla användare i systemet. Kräver admin-roll.";
                s.Response<IEnumerable<User>>(200, "Lista med användare");
                s.Response(401, "Ej inloggad");
                s.Response(403, "Inte behörig (kräver admin)");
            });
        }

    }
}
