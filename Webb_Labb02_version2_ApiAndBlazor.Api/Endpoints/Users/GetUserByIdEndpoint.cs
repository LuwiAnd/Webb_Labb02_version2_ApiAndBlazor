using FastEndpoints;
using Webb_Labb02_version2_ApiAndBlazor.Api.Entities;
using Webb_Labb02_version2_ApiAndBlazor.Api.Repositories.Interfaces;
using Webb_Labb02_version2_ApiAndBlazor.Api.Models.RequestDto;
using Webb_Labb02_version2_ApiAndBlazor.Api.Models.ResponseDto;

namespace Webb_Labb02_version2_ApiAndBlazor.Api.Endpoints.Users
{
    //[HttpGet("/users/{id}")]
    //public class GetUserByIdEndpoint : Endpoint<GetUserByIdRequest, User>
    //public class GetUserByIdEndpoint : EndpointWithoutRequest
    public class GetUserByIdEndpoint : EndpointWithoutRequest<UserResponse>
    {
        private readonly IUnitOfWork _uow;

        public GetUserByIdEndpoint(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public override void Configure()
        {
            Get("users/{id}");
            //Post("users/getbyid");
            //AllowGet(); // <- detta var i en gammal FastEndpoint-version.
            
            //Roles("admin");
            AllowAnonymous(); // För testning.

            Version(1);
            Options(x => x.WithName("GetUserById"));

            Summary(s =>
            {
                s.Summary = "Hämtar en användare via ID (admin-only)";
                s.Description = "Endast admin får tillgång till denna resurs.";

                s.Params["id"] = "ID för användaren som ska hämtas";

                s.Response<User>(200, "Hittad användare");
                s.Response(404, "Användare hittades inte");
                s.Response(401, "Ej inloggad");
                s.Response(403, "Inte behörig (kräver admin)");
            });
        }

        /* Detta var innan jag bytte till EndpointWithoutRequest
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
        */

        /*
        public override async Task HandleAsync(CancellationToken ct)
        {
            var id = Route<int>("id");

            var user = await _uow.Users.GetByIdAsync(id);
            if (user is null)
            {
                await SendNotFoundAsync(ct);
                return;
            }

            await SendAsync(user, cancellation: ct);
        }
        */

        public override async Task HandleAsync(CancellationToken ct)
        {
            var id = Route<int>("id");

            var user = await _uow.Users.GetByIdAsync(id);
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
