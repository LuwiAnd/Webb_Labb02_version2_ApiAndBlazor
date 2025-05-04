using Webb_Labb02_version2_ApiAndBlazor.Api.Entities;

namespace Webb_Labb02_version2_ApiAndBlazor.Api.Models.RequestDto
{
    public class UpdateProductRequest
    {
        //public string? ProductName { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public string? Category { get; set; }
        public ProductStatus? Status { get; set; }

        public int? StockQuantity { get; set; }

        public int? ProductNumber { get; set; }
    }
}
