using FastEndpoints;
using Webb_Labb02_version2_ApiAndBlazor.Api.Repositories.Interfaces;

namespace Webb_Labb02_version2_ApiAndBlazor.Api.Endpoints.Products;

public class DeleteProductByNumberEndpoint : EndpointWithoutRequest
{
    private readonly IUnitOfWork _uow;

    public DeleteProductByNumberEndpoint(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public override void Configure()
    {
        Delete("products/by-number/{number}");
        Roles("admin");

        Summary(s =>
        {
            s.Summary = "Tar bort en produkt baserat på produktnummer";
            s.Description = "Endast tillgänglig för admin";
            s.Params["number"] = "Produktnummer";
            s.Response(204, "Borttagen");
            s.Response(404, "Produkten hittades inte");
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var number = Route<int>("number");

        var product = await _uow.Products.GetByProductNumberAsync(number);
        if (product is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await _uow.Products.DeleteAsync(product.ID); // vi använder ID internt
        await _uow.CompleteAsync();

        await SendNoContentAsync(ct);
    }
}
