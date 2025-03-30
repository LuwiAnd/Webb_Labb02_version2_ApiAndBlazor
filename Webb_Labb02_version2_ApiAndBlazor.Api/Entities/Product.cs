namespace Webb_Labb02_version2_ApiAndBlazor.Api.Entities
{
    public class Product
    {
        public int ProductID { get; set; }
        public int ProductNumber { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? ProductDescription { get; set; }
        public decimal Price { get; set; }
        public string ProductCategory { get; set; } = string.Empty;
        public string ProductStatus { get; set; } = string.Empty;
    }
}
