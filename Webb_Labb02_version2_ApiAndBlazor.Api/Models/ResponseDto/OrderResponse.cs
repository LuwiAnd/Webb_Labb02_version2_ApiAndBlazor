namespace Webb_Labb02_version2_ApiAndBlazor.Api.Models.ResponseDto
{
    //public class OrderResponse
    //{
    //    public int OrderID { get; set; }
    //    public int UserID { get; set; }
    //    public DateTime OrderDate { get; set; }
    //    public string OrderStatus { get; set; } = string.Empty;
    //    public decimal TotalAmount { get; set; }

    //    public List<OrderItemResponse> Items { get; set; } = new();
    //}

    public class OrderResponse
    {
        public int OrderID { get; set; }
        public int UserID { get; set; }
        public string Email { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public List<OrderItemResponse> Items { get; set; } = new();
    }

}
