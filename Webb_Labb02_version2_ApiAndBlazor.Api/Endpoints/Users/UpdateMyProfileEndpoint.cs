using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using FastEndpoints.Security;
using Webb_Labb02_version2_ApiAndBlazor.Api.Entities;
using Webb_Labb02_version2_ApiAndBlazor.Api.Models.RequestDto;
using Webb_Labb02_version2_ApiAndBlazor.Api.Repositories.Interfaces;
using System.Security.Claims;

namespace Webb_Labb02_version2_ApiAndBlazor.Api.Endpoints.Users
{
    public class UpdateMyProfileEndpoint : Endpoint<UpdateMyProfileRequest>
    {
        private readonly IUnitOfWork _uow;

        public UpdateMyProfileEndpoint(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public override void Configure()
        {
            Put("/users/me");
            Roles("user", "admin");

            Summary(s =>
            {
                s.Summary = "Uppdaterar den inloggade användarens profil";
                s.Description = "Endast tillåtna fält kan uppdateras. Roll kan inte ändras.";
                s.Response(204, "Uppdatering lyckades");
                s.Response(400, "Ogiltig data");
                s.Response(401, "Ej inloggad");
            });
        }

        public override async Task HandleAsync(UpdateMyProfileRequest req, CancellationToken ct)
        {
            var userIdClaim = User.ClaimValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out var userId))
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

            // Uppdatera endast fält som inte är null eller tomma
            if (!string.IsNullOrWhiteSpace(req.FirstName))
                user.FirstName = req.FirstName;

            if (!string.IsNullOrWhiteSpace(req.LastName))
                user.LastName = req.LastName;

            if (!string.IsNullOrWhiteSpace(req.Email))
                user.Email = req.Email;

            if (!string.IsNullOrWhiteSpace(req.PhoneNumber))
                user.PhoneNumber = req.PhoneNumber;

            if (!string.IsNullOrWhiteSpace(req.HomeAddress))
                user.HomeAddress = req.HomeAddress;

            if (!string.IsNullOrWhiteSpace(req.Password))
            {
                var hasher = new PasswordHasher<User>();
                user.PasswordHash = hasher.HashPassword(user, req.Password);
            }

            await _uow.Users.UpdateAsync(user);
            await _uow.CompleteAsync();

            await SendNoContentAsync(ct);
        }
    }
}
