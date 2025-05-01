using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using Webb_Labb02_version2_ApiAndBlazor.Api.Entities;
using Webb_Labb02_version2_ApiAndBlazor.Api.Models.RequestDto;
using Webb_Labb02_version2_ApiAndBlazor.Api.Models.ResponseDto;
using Webb_Labb02_version2_ApiAndBlazor.Api.Repositories.Interfaces;

namespace Webb_Labb02_version2_ApiAndBlazor.Api.Endpoints.Users
{
    //public class RegisterUserEndpoint : Endpoint<RegisterUserRequest, User>
    public class RegisterUserEndpoint : Endpoint<RegisterUserRequest, UserResponse>
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

            var existing = await _uow.Users.GetByEmailAsync(req.Email);
            if (existing is not null)
            {
                AddError(r => r.Email, "E-postadressen är redan registrerad.");
                await SendErrorsAsync(400, ct);
                return;
            }



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

            //await SendCreatedAtAsync<GetUserByIdEndpoint>(
            //    new { id = user.UserID },
            //    user,
            //    cancellation: ct
            //);

            await SendCreatedAtAsync<GetUserByIdEndpoint>(
                new { id = user.UserID },
                new UserResponse
                {
                    UserID = user.UserID,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email ?? "",
                    PhoneNumber = user.PhoneNumber ?? "",
                    HomeAddress = user.HomeAddress ?? "",
                    Role = user.Role
                },
                cancellation: ct
            );

        }
    }
}
