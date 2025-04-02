using FastEndpoints;
using Webb_Labb02_version2_ApiAndBlazor.Api.Entities;
using Webb_Labb02_version2_ApiAndBlazor.Api.Repositories.Interfaces;
using Webb_Labb02_version2_ApiAndBlazor.Api.Models.RequestDto;

namespace Webb_Labb02_version2_ApiAndBlazor.Api.Endpoints.Users
{
    public class GetUserByIdEndpoint : Endpoint<GetUserByIdRequest, User>
    {
        private readonly IUnitOfWork _uow;

        public GetUserByIdEndpoint(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public override void Configure()
        {
            Get("/users/{id}");
            Roles("admin");

            Summary(s =>
            {
                s.Summary = "Hämtar en användare via ID (admin-only)";
                s.Description = "Endast admin får tillgång till denna resurs.";
                s.Response<User>(200, "Hittad användare");
                s.Response(404, "Användare hittades inte");
                s.Response(401, "Ej inloggad");
                s.Response(403, "Inte behörig (kräver admin)");
            });
        }

        public override async Task HandleAsync(GetUserByIdRequest req, CancellationToken ct)
        {
            var user = await _uow.Users.GetByIdAsync(req.Id);

            if(user is null)
            {
                await SendNotFoundAsync(ct);
                return;
            }

            await SendAsync(user, cancellation: ct);
        }


    }
}
