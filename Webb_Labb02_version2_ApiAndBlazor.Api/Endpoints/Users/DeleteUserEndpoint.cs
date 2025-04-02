using FastEndpoints;
using Webb_Labb02_version2_ApiAndBlazor.Api.Models.RequestDto;
using Webb_Labb02_version2_ApiAndBlazor.Api.Repositories.Interfaces;

namespace Webb_Labb02_version2_ApiAndBlazor.Api.Endpoints.Users
{
    public class DeleteUserEndpoint : Endpoint<DeleteUserRequest>
    {
        private readonly IUnitOfWork _uow;

        public DeleteUserEndpoint(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public override void Configure()
        {
            Delete("/users/{id}");
            Roles("admin");

            Summary(s =>
            {
                s.Summary = "Tar bort en användare";
                s.Description = "Endast admin kan ta bort en användare baserat på ID.";
                s.Response(204, "Användare borttagen");
                s.Response(404, "Användare hittades inte");
                s.Response(401, "Ej inloggad");
                s.Response(403, "Inte behörig");
            });
        }

        public override async Task HandleAsync(DeleteUserRequest req, CancellationToken ct)
        {
            var user = await _uow.Users.GetByIdAsync(req.Id);

            if(user is null)
            {
                await SendNotFoundAsync(ct);
                return;
            }

            await _uow.Users.DeleteAsync(user.UserID);
            await _uow.CompleteAsync();

            await SendNoContentAsync(ct);
        }
    }
}
