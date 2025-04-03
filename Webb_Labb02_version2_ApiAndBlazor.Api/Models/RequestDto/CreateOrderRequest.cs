namespace Webb_Labb02_version2_ApiAndBlazor.Api.Models.RequestDto
{
    public class CreateOrderRequest
    {
        public int UserID { get; set; }
        public List<OrderItemRequest> Items { get; set; } = new();
    }
}
