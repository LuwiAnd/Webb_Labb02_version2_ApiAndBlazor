using FastEndpoints;
using Webb_Labb02_version2_ApiAndBlazor.Api.Repositories.Interfaces;
using Webb_Labb02_version2_ApiAndBlazor.Api.Models.RequestDto;
using Webb_Labb02_version2_ApiAndBlazor.Api.Entities;

namespace Webb_Labb02_version2_ApiAndBlazor.Api.Endpoints.Products
{
    public class UpdateProductEndpoint : Endpoint<UpdateProductRequest>
    {
        private readonly IUnitOfWork _uow;

        public UpdateProductEndpoint(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public override void Configure()
        {
            Put("products/{id}");
            Roles("admin");

            Summary(s =>
            {
                s.Summary = "Uppdaterar en produkt";
                s.Description = "Endast angivna fält uppdateras. Tomma/null fält lämnas oförändrade.";
                s.Params["id"] = "ID för produkten som ska uppdateras";
                s.Response(204, "Produkt uppdaterad");
                s.Response(404, "Produkt hittades inte");
                s.Response(400, "Felaktig data");
            });
        }

        public override async Task HandleAsync(UpdateProductRequest req, CancellationToken ct)
        {
            var id = Route<int>("id");

            var product = await _uow.Products.GetByIdAsync(id);
            if (product is null)
            {
                await SendNotFoundAsync(ct);
                return;
            }

            // Endast uppdatera om värdet inte är null eller tomt
            if (!string.IsNullOrWhiteSpace(req.Name))
                product.Name = req.Name;

            if (!string.IsNullOrWhiteSpace(req.Description))
                product.Description = req.Description;

            if (req.Price.HasValue)
                product.Price = req.Price.Value;

            if (!string.IsNullOrWhiteSpace(req.Category))
                product.Category = req.Category;

            if (req.Status.HasValue)
                product.Status = req.Status.Value;

            if (req.StockQuantity.HasValue)
                product.StockQuantity = req.StockQuantity.Value;


            await _uow.Products.UpdateAsync(product);
            await _uow.CompleteAsync();

            await SendNoContentAsync(ct);
        }
    }
}
