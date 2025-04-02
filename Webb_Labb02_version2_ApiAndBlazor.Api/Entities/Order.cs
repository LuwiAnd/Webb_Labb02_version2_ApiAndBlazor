using System.ComponentModel.DataAnnotations;

namespace Webb_Labb02_version2_ApiAndBlazor.Api.Entities
{
    public class Order
    {
        /* 
            CREATE TABLE Orders (
                OrderID INT IDENTITY(1,1) PRIMARY KEY,
                UserID INT NOT NULL,
                OrderDate DATETIME DEFAULT GETDATE(),
	            OrderStatus NVARCHAR(50) NOT NULL CHECK (OrderStatus IN ('pending', 'shipped', 'delivered', 'cancelled', 'returned')),
                FOREIGN KEY (UserID) REFERENCES Users(UserID) ON DELETE CASCADE
            );
        */
        [Key]
        public int OrderID { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new();


        public int UserID { get; set; }
        public User? User { get; set; }


        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public string OrderStatus { get; set; } = string.Empty;

        public decimal TotalAmount { get; set; }
    }
}
