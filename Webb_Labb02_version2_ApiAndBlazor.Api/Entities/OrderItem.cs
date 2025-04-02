using System.ComponentModel.DataAnnotations;

namespace Webb_Labb02_version2_ApiAndBlazor.Api.Entities
{
    public class OrderItem
    {
        /* 
            -- Junction table between Orders and Products
            CREATE TABLE OrderItems (
                OrderItemID INT IDENTITY(1,1) PRIMARY KEY,
                OrderID INT NOT NULL,
                ProductID INT NOT NULL,
                Quantity INT NOT NULL CHECK (Quantity > 0),
                Price DECIMAL(10,2) NOT NULL,
                FOREIGN KEY (OrderID) REFERENCES Orders(OrderID) ON DELETE CASCADE,
                FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
            );
        */

        [Key]
        public int OrderItemID { get; set; }

        [Required]
        public int OrderID { get; set; }
        public Order? Order { get; set; }

        [Required]
        public int ProductID { get; set; }
        public Product? Product { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
