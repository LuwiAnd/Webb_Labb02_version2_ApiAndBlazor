using FastEndpoints;
using Webb_Labb02_version2_ApiAndBlazor.Api.Repositories.Interfaces;

namespace Webb_Labb02_version2_ApiAndBlazor.Api.Endpoints.Products;

public class DeleteProductEndpoint : EndpointWithoutRequest
{
    private readonly IUnitOfWork _uow;

    public DeleteProductEndpoint(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public override void Configure()
    {
        Delete("products/{id}");
        Roles("admin");

        Summary(s =>
        {
            s.Summary = "Tar bort en produkt baserat på dess ID";
            s.Params["id"] = "Produktens ID";
            s.Response(204, "Borttaget");
            s.Response(404, "Produkten hittades inte");
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<int>("id");
        var product = await _uow.Products.GetByIdAsync(id);

        if (product is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await _uow.Products.DeleteAsync(id);
        await _uow.CompleteAsync();

        await SendNoContentAsync(ct);
    }
}
