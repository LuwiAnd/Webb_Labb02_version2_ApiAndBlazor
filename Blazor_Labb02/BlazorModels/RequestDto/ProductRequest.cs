using System.ComponentModel.DataAnnotations;
using Blazor_Labb02.SharedModels.Enums;

namespace Blazor_Labb02.BlazorModels.RequestDto
{
    public class ProductRequest
    {
        [Required]
        public int Number { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Range(0, 1_000_000_000, ErrorMessage = "Priset måste vara mellan 0 och en miljard!")]
        public decimal Price { get; set; }

        [Required]
        public string Category { get; set; } = string.Empty;

        [Required]
        public ProductStatus Status { get; set; } = ProductStatus.Available;

        [Range(0, int.MaxValue, ErrorMessage = "Lagersaldo får inte vara negativt.")]
        public int StockQuantity { get; set; } = 0;


    }
}
