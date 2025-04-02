using FastEndpoints;
using Webb_Labb02_version2_ApiAndBlazor.Api.Entities;
using Webb_Labb02_version2_ApiAndBlazor.Api.Models.RequestDto;
using Webb_Labb02_version2_ApiAndBlazor.Api.Repositories.Interfaces;

namespace Webb_Labb02_version2_ApiAndBlazor.Api.Endpoints.Users
{
    public class UpdateUserByAdminEndpoint : Endpoint<UpdateUserRequest, User>
    {
        private readonly IUnitOfWork _uow;

        public UpdateUserByAdminEndpoint(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public override void Configure()
        {
            Put("/users");
            Roles("admin");

            Summary(s =>
            {
                s.Summary = "Uppdaterar en användare";
                s.Description = "Endast tillgänglig för administratörer.";
                s.Response<User>(200, "Uppdaterad användare");
                s.Response(404, "Användaren hittades inte");
            });
        }

        public override async Task HandleAsync(UpdateUserRequest req, CancellationToken ct)
        {
            var user = await _uow.Users.GetByIdAsync(req.Id);

            if(user is null)
            {
                await SendNotFoundAsync();
                return;
            }

            user.FirstName = req.FirstName;
            user.LastName = req.LastName;
            user.Email = req.Email;
            user.PhoneNumber = req.PhoneNumber;
            user.HomeAddress = req.HomeAddress;
            user.Role = req.Role ?? user.Role;

            await _uow.CompleteAsync();
            await SendOkAsync(user, ct);
        }
    }
}
