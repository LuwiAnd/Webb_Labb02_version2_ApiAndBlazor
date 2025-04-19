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

        [Range(0.01, 999999.99)]
        public decimal Price { get; set; }

        [Required]
        public string Category { get; set; } = string.Empty;

        [Required]
        public ProductStatus Status { get; set; } = ProductStatus.Available;
    }
}
