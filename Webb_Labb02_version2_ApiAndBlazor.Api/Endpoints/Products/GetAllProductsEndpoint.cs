using FastEndpoints;
using Webb_Labb02_version2_ApiAndBlazor.Api.Entities;
using Webb_Labb02_version2_ApiAndBlazor.Api.Repositories.Interfaces;

namespace Webb_Labb02_version2_ApiAndBlazor.Endpoints.Products
{
    public class GetAllProductsEndpoint : EndpointWithoutRequest<IEnumerable<Product>>
    {
        private readonly IProductRepository _repository;

        public GetAllProductsEndpoint(IProductRepository repository)
        {
            _repository = repository;
        }

        public override void Configure()
        {
            Get("/products");
            AllowAnonymous();

            //Group = "Produkter";

            Summary(s =>
            {
                s.Summary = "Hämtar alla produkter";
                s.Description = "Returnerar en lista med alla produkter i databasen.";
                s.Response<IEnumerable<Product>>(200, "Lista med produkter");
            });
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var products = await _repository.GetAllAsync();
            await SendAsync(products, cancellation: ct);
        }
    }
}
