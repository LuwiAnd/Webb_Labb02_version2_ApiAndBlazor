using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using Webb_Labb02_version2_ApiAndBlazor.Api.Entities;
using Webb_Labb02_version2_ApiAndBlazor.Api.Models.RequestDto;
using Webb_Labb02_version2_ApiAndBlazor.Api.Repositories.Interfaces;

namespace Webb_Labb02_version2_ApiAndBlazor.Api.Endpoints.Users
{
    public class RegisterUserEndpoint : Endpoint<RegisterUserRequest, User>
    {
        private readonly IUnitOfWork _uow;

        public RegisterUserEndpoint(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public override void Configure()
        {
            Post("/register");
            AllowAnonymous();

            Summary(s =>
            {
                s.Summary = "Registrerar en ny användare";
                s.Description = "Tillgänglig för icke-inloggade användare. Skapar alltid med rollen 'user'.";
                s.Response<User>(201, "Ny användare skapad");
                s.Response(400, "Ogiltig data");
            });
        }

        public override async Task HandleAsync(RegisterUserRequest req, CancellationToken ct)
        {
            var hasher = new PasswordHasher<User>();

            var user = new User
            {
                FirstName = req.FirstName,
                LastName = req.LastName,
                Email = req.Email,
                PhoneNumber = req.PhoneNumber,
                HomeAddress = req.HomeAddress,
                Password = req.Password,
                PasswordHash = "",
                Role = "user"
            };

            user.PasswordHash = hasher.HashPassword(user, req.Password);

            await _uow.Users.AddAsync(user);
            await _uow.CompleteAsync();

            await SendCreatedAtAsync<GetUserByIdEndpoint>(
                new { id = user.UserID },
                user,
                cancellation: ct
            );
        }
    }
}
