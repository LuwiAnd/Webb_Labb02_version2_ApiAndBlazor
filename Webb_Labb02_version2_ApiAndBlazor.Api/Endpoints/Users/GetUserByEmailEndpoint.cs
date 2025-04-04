using FastEndpoints;
using Webb_Labb02_version2_ApiAndBlazor.Api.Models.ResponseDto;
using Webb_Labb02_version2_ApiAndBlazor.Api.Repositories.Interfaces;
using Webb_Labb02_version2_ApiAndBlazor.Api.Models.RequestDto;

namespace Webb_Labb02_version2_ApiAndBlazor.Api.Endpoints.Users
{
    public class GetUserByEmailEndpoint : Endpoint<GetUserByEmailRequest, UserResponse>
    {
        private readonly IUnitOfWork _uow;

        public GetUserByEmailEndpoint(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public override void Configure()
        {
            Get("users/search");
            Roles("admin");

            Summary(s =>
            {
                s.Summary = "Hämtar användare via e-postadress";
                s.Description = "Returnerar en användare med exakt matchande e-post.";
                s.Params["email"] = "E-postadress att söka efter";
                s.Response<UserResponse>(200, "Användare hittad");
                s.Response(404, "Användare hittades inte");
            });
        }

        public override async Task HandleAsync(GetUserByEmailRequest req, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(req.Email))
            {
                AddError(r => r.Email, "E-post måste anges som queryparameter.");
                await SendErrorsAsync(400, ct);
                return;
            }

            var user = await _uow.Users.GetByEmailAsync(req.Email);

            if (user is null)
            {
                await SendNotFoundAsync(ct);
                return;
            }

            var response = new UserResponse
            {
                UserID = user.UserID,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email ?? "",
                PhoneNumber = user.PhoneNumber ?? "",
                HomeAddress = user.HomeAddress,
                Role = user.Role
            };

            await SendAsync(response, cancellation: ct);
        }
    }
}
