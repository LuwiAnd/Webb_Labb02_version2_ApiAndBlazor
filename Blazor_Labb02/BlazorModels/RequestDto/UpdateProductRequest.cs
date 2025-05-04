using Blazor_Labb02.SharedModels.Enums;

namespace Blazor_Labb02.BlazorModels.RequestDto
{
    //public enum ProductStatus
    //{
    //    Available,
    //    Discontinued,
    //    OutOfStock
    //}
    public class UpdateProductRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public string? Category { get; set; }
        public ProductStatus? Status { get; set; }

        public int StockQuantity { get; set; }

    }
}


