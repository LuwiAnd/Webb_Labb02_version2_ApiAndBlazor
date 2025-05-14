using Webb_Labb02_version2_ApiAndBlazor.Api.Entities;

namespace Webb_Labb02_version2_ApiAndBlazor.Api.Models.RequestDto
{
    public class ProductRequest
    {
        public int Number { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; } = string.Empty;
        //public string ProductStatus { get; set; } = string.Empty;
        public ProductStatus Status { get; set; } = ProductStatus.Available;

        public int StockQuantity { get; set; } = 0;
    }
}
