using Blazor_Labb02.SharedModels.Enums;
using System.ComponentModel.DataAnnotations;

namespace Blazor_Labb02.BlazorModels.ResponseDto;

//public enum ProductStatus
//{
//    Available,
//    Discontinued,
//    OutOfStock
//}
public class ProductResponse
{
    public int Id { get; set; }
    [Required]
    public int Number { get; set; }
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    [Range(0, 1_000_000_000, ErrorMessage = "Priset måste vara mellan 0 och en miljard!")]
    public decimal Price { get; set; }
    [Required]
    public string Category { get; set; } = string.Empty;
    //public bool IsDiscontinued { get; set; }
    //public string Status { get; set; } = "Available";
    [Required]
    public ProductStatus Status { get; set; } = ProductStatus.Available;

    public int StockQuantity { get; set; }
}
