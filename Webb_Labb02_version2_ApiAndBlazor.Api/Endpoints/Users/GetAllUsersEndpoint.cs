using FastEndpoints;
using Webb_Labb02_version2_ApiAndBlazor.Api.Entities;
using Webb_Labb02_version2_ApiAndBlazor.Api.Repositories.Interfaces;
using Webb_Labb02_version2_ApiAndBlazor.Api.Models.ResponseDto;

namespace Webb_Labb02_version2_ApiAndBlazor.Endpoints.Users
{
    //public class GetAllUsersEndpoint : EndpointWithoutRequest<IEnumerable<User>>
    public class GetAllUsersEndpoint : EndpointWithoutRequest<IEnumerable<UserResponse>>
    {
        private readonly IUnitOfWork _uow;

        public GetAllUsersEndpoint(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var users = await _uow.Users.GetAllAsync();

            var response = users.Select(u => new UserResponse
            {
                UserID = u.UserID,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email ?? "",
                PhoneNumber = u.PhoneNumber ?? "",
                HomeAddress = u.HomeAddress,
                Role = u.Role
            }).ToList();

            //await SendAsync(users);
            await SendAsync(response, cancellation: ct);
        }

        public override void Configure()
        {
            Get("/users");
            Roles("admin");

            Summary(s =>
            {
                s.Summary = "Hämtar alla användare (endast för admin)";
                s.Description = "Returnerar en lista av alla användare i systemet. Kräver admin-roll.";
                //s.Response<IEnumerable<User>>(200, "Lista med användare");
                s.Response<IEnumerable<UserResponse>>(200, "Lista med användare");
                s.Response(401, "Ej inloggad");
                s.Response(403, "Inte behörig (kräver admin)");
            });
        }

    }
}
