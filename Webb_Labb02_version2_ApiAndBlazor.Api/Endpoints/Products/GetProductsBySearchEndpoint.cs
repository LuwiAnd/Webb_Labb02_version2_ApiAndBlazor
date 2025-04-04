using FastEndpoints;
using Webb_Labb02_version2_ApiAndBlazor.Api.Models.RequestDto;
using Webb_Labb02_version2_ApiAndBlazor.Api.Repositories.Interfaces;
using Webb_Labb02_version2_ApiAndBlazor.Api.Entities;

namespace Webb_Labb02_version2_ApiAndBlazor.Api.Endpoints.Products
{
    public class GetProductsBySearchEndpoint : Endpoint<SearchProductsRequest, IEnumerable<Product>>
    {
        private readonly IProductRepository _repository;

        public GetProductsBySearchEndpoint(IProductRepository repository)
        {
            _repository = repository;
        }

        public override void Configure()
        {
            Get("products/search");
            AllowAnonymous();

            Summary(s =>
            {
                s.Summary = "Sök produkter";
                s.Description = "Söker efter produkter via namn eller produktnummer";
                s.Params["name"] = "Produktnamn eller del av namn";
                s.Params["productNumber"] = "Exakt produktnummer";
                s.Response<IEnumerable<Product>>(200, "Matchande produkter");
            });
        }

        public override async Task HandleAsync(SearchProductsRequest req, CancellationToken ct)
        {
            var results = await _repository.SearchAsync(req.Name, req.ProductNumber);
            await SendAsync(results, cancellation: ct);
        }
    }
}
