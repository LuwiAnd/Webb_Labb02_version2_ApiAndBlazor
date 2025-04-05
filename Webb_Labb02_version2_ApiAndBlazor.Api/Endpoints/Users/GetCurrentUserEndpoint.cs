using FastEndpoints;
using FastEndpoints.Security;
using Webb_Labb02_version2_ApiAndBlazor.Api.Repositories.Interfaces;
using Webb_Labb02_version2_ApiAndBlazor.Api.Models.ResponseDto;
using System.Security.Claims;

namespace Webb_Labb02_version2_ApiAndBlazor.Api.Endpoints.Users
{
    public class GetCurrentUserEndpoint : EndpointWithoutRequest<UserResponse>
    {
        private readonly IUnitOfWork _uow;

        public GetCurrentUserEndpoint(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public override void Configure()
        {
            Get("users/me");
            Roles("user", "admin");

            Summary(s =>
            {
                s.Summary = "Hämtar information om den inloggade användaren";
                s.Description = "Returnerar användarens egna profilinformation";
                s.Response<UserResponse>(200, "Användarinfo");
                s.Response(401, "Ej inloggad");
            });
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            //var userIdStr = User.ClaimValue("sub");
            var userIdStr = User.ClaimValue(ClaimTypes.NameIdentifier);


            if (!int.TryParse(userIdStr, out int userId))
            {
                await SendUnauthorizedAsync(ct);
                return;
            }

            var user = await _uow.Users.GetByIdAsync(userId);

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
                Role = user.Role // du kan utesluta den här om det ska döljas
            };

            await SendAsync(response, cancellation: ct);
        }
    }
}
