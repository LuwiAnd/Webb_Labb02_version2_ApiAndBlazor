using FastEndpoints;
using Webb_Labb02_version2_ApiAndBlazor.Api.Entities;
using Webb_Labb02_version2_ApiAndBlazor.Api.Models.RequestDto;
using Webb_Labb02_version2_ApiAndBlazor.Api.Repositories.Interfaces;

namespace Webb_Labb02_version2_ApiAndBlazor.Api.Endpoints.Products
{

    // Jag kunde inte använda int, för Swagger gav felmeddelande då.
    //public class GetProductByIdEndpoint : Endpoint<int, Product> 
    public class GetProductByIdEndpoint : Endpoint<GetProductByIdRequest, Product>
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

        public override async Task HandleAsync(GetProductByIdRequest req, CancellationToken ct)
        {
            var product = await _repository.GetByIdAsync(req.Id);

            if(product is null)
            {
                await SendNotFoundAsync(ct);
                return;
            }

            await SendAsync(product, cancellation: ct);
        }
    }
}
