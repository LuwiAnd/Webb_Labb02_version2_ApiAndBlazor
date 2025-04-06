namespace Blazor_Labb02.BlazorModels.ResponseDto;

public enum ProductStatus
{
    Available,
    Discontinued,
    OutOfStock
}
public class ProductResponse
{
    public int Id { get; set; }
    public int Number { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Category { get; set; } = string.Empty;
    //public bool IsDiscontinued { get; set; }
    //public string Status { get; set; } = "Available";
    public ProductStatus Status { get; set; } = ProductStatus.Available;
}
