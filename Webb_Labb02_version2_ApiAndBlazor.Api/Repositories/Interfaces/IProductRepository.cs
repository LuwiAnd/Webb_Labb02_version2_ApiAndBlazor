using Webb_Labb02_version2_ApiAndBlazor.Api.Entities;

namespace Webb_Labb02_version2_ApiAndBlazor.Api.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);

        Task<IEnumerable<Product>> SearchAsync(string? name, string? productNumber);

        Task<Product?> GetByProductNumberAsync(int productNumber);

        // Kontrollerar att det inte finns någon produkt i databasen med ett visst 
        // produktnummer. Används för att kunna uppdatera produktnummer i ProductEdit.razor.
        Task<bool> AnyWithProductNumberAsync(int productNumber);



    }
}
