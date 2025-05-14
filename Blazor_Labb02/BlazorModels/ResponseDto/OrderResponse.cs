using System;
using System.Collections.Generic;

namespace Blazor_Labb02.BlazorModels.ResponseDto
{
    public class OrderResponse
    {
        public int OrderID { get; set; }
        public int UserID { get; set; }
        public string? Email { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public List<OrderItemResponse> Items { get; set; } = new();
    }
}
