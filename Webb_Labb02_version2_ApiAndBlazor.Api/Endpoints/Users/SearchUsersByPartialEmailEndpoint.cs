using FastEndpoints;
using Webb_Labb02_version2_ApiAndBlazor.Api.Repositories.Interfaces;
using Webb_Labb02_version2_ApiAndBlazor.Api.Models.ResponseDto;

namespace Webb_Labb02_version2_ApiAndBlazor.Api.Endpoints.Users;

public class SearchUsersByEmailFragmentEndpoint : EndpointWithoutRequest<List<UserResponse>>
{
    private readonly IUnitOfWork _uow;

    public SearchUsersByEmailFragmentEndpoint(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public override void Configure()
    {
        Get("users/search/fragment");
        Roles("admin");

        Summary(s =>
        {
            s.Summary = "Söker användare vars e-post innehåller en viss text";
            s.Description = "Returnerar alla användare där e-posten innehåller den angivna delsträngen.";
            s.Params["emailFragment"] = "Del av e-postadress att söka efter";
            s.Response(200, "Lista med användare");
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        //var partialEmail = Query<string>("emailFragment");
        var partialEmail = Query<string>("partialEmail");


        if (string.IsNullOrWhiteSpace(partialEmail))
        {
            await SendErrorsAsync(400, ct);
            return;
        }

        var users = await _uow.Users.SearchByPartialEmailAsync(partialEmail);

        var response = users.Select(user => new UserResponse
        {
            UserID = user.UserID,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email ?? "",
            PhoneNumber = user.PhoneNumber ?? "",
            HomeAddress = user.HomeAddress ?? "",
            Role = user.Role
        }).ToList();

        await SendAsync(response, cancellation: ct);
    }
}
