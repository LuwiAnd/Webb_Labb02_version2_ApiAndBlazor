using FastEndpoints;
using Webb_Labb02_version2_ApiAndBlazor.Api.Entities;
using Webb_Labb02_version2_ApiAndBlazor.Api.Models.RequestDto;
using Webb_Labb02_version2_ApiAndBlazor.Api.Repositories.Interfaces;
using Webb_Labb02_version2_ApiAndBlazor.Api.Endpoints.Products;

namespace Webb_Labb02_version2_ApiAndBlazor.Api.Endpoints.Products
{
    public class CreateProductEndpoint : Endpoint<ProductRequest, Product>
    {
        private readonly IProductRepository _repository;

        public CreateProductEndpoint(IProductRepository repository)
        {
            _repository = repository;
        }

        public override void Configure()
        {
            Post("/products");
            AllowAnonymous();

            Summary(s =>
            {
                s.Summary = "Lägger till en ny produkt";
                s.Description = "Tar emot en produkt och sparar den i databasen.";
                s.Response<Product>(201, "Returnerar den skapade produkten");
            });
        }

        public override async Task HandleAsync(ProductRequest req, CancellationToken ct)
        {
            Product product = new Product
            {
                ProductNumber = req.ProductNumber,
                ProductName = req.ProductName,
                ProductDescription = req.ProductDescription,
                Price = req.Price,
                ProductCategory = req.ProductCategory,
                ProductStatus = req.ProductStatus
            };

            await _repository.AddAsync(product);

            //Jag har inte gjort GetProductByIdEndpoint än, så denna kod kan jag implementera senare.
            //await SendCreatedAtAsync<GetProductByIdEndpoint, Product>(
            //    $"/products/{product.ProductID}",
            //    product,
            //    cancellation: ct
            //);
            //Istället för ovanstående så använder jag följande kodrad så länge.
            //await SendAsync(product, 201, cancellation: ct);
            // Efter att ha skapat GetProductByIdEndpoint gör jag ett nytt försök:
            //await FastEndpoints.EndpointExtensions.SendCreatedAtAsync<GetProductByIdEndpoint, Product>(
            //    this,
            //    new { id = product.ProductID },
            //    product,
            //    cancellation: ct
            //);
            // Ovanstående fungerade inte heller, för det verkar som att metoden med två typer inte finns i 
            // den version av FastEndpoints som jag installerat.
            await SendAsync(product, 201, cancellation: ct);

        }

    }
}
