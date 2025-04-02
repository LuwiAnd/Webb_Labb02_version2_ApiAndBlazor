using FastEndpoints;
using Webb_Labb02_version2_ApiAndBlazor.Api.Entities;
using Webb_Labb02_version2_ApiAndBlazor.Api.Models.RequestDto;
using Webb_Labb02_version2_ApiAndBlazor.Api.Repositories.Interfaces;

// För hashning:
using Microsoft.AspNetCore.Identity;

namespace Webb_Labb02_version2_ApiAndBlazor.Api.Endpoints.Users
{
    public class CreateUserByAdminEndpoint : Endpoint<CreateUserRequest, User>
    {
        private readonly IUnitOfWork _uow;

        public CreateUserByAdminEndpoint(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public override void Configure()
        {
            Post("/users");
            Roles("admin");

            Summary(s =>
            {
                s.Summary = "Skapar en ny användare";
                s.Description = "Endast tillgänglig för användare med admin-roll.";
                s.Response<User>(201, "Skapad användare");
                s.Response(400, "Ogiltig data");
                s.Response(401, "Ej inloggad");
                s.Response(403, "Inte behörig");
            });
        }

        public override async Task HandleAsync(CreateUserRequest req, CancellationToken ct)
        {
            var hasher = new PasswordHasher<User>();
            var user = new User
            {
                FirstName = req.FirstName,
                LastName = req.LastName,
                Email = req.Email,
                PhoneNumber = req.PhoneNumber,
                HomeAddress = req.HomeAddress,
                Role = req.Role,
                Password = req.Password,
                PasswordHash = "" // ev. hashing kan göras här
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
