namespace Webb_Labb02_version2_ApiAndBlazor.Api.Models.ResponseDto
{
    public class CartItemResponse
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int StockQuantity { get; set; }
    }
}
