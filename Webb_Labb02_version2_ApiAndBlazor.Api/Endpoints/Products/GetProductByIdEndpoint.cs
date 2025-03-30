using FastEndpoints;
using Webb_Labb02_version2_ApiAndBlazor.Api.Entities;
using Webb_Labb02_version2_ApiAndBlazor.Api.Repositories.Interfaces;

namespace Webb_Labb02_version2_ApiAndBlazor.Api.Endpoints.Products
{

    public class GetProductByIdEndpoint : Endpoint<int, Product>
    {
        private readonly IProductRepository _repository;

        public GetProductByIdEndpoint(IProductRepository repository)
        {
            _repository = repository;
        }

        public override void Configure()
        {
            Get("/products/{id}");
            AllowAnonymous();

            Summary(s =>
            {
                s.Summary = "Hämtar en produkt med ett specifikt ID";
                s.Description = "Returnerar en produkt om den finns, annars 404.";
                s.Response<Product>(200, "Hittad produkt");
                s.Response(404, "Ingen produkt hittades");
            });
        }

        public override async Task HandleAsync(int id, CancellationToken ct)
        {
            var product = await _repository.GetByIdAsync(id);

            if(product is null)
            {
                await SendNotFoundAsync(ct);
                return;
            }

            await SendAsync(product, cancellation: ct);
        }
    }
}
