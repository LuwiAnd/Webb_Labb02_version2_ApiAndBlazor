using Microsoft.EntityFrameworkCore;
using Webb_Labb02_version2_ApiAndBlazor.Api.Data;
using Webb_Labb02_version2_ApiAndBlazor.Api.Entities;
using Webb_Labb02_version2_ApiAndBlazor.Api.Repositories.Interfaces;


namespace Webb_Labb02_version2_ApiAndBlazor.Api.Repositories.Implementations
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if(product is not null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Products.AnyAsync(p => p.ID == id);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }


        public async Task<IEnumerable<Product>> SearchAsync(string? name, string? productNumber)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
            {
                //query = query.Where(p => p.ProductName.Contains(name));
                //query = query.Where(p => p.ProductName.ToLower().Contains(name.ToLower()));
                query = query.Where(p => EF.Functions.Like(p.Name, $"%{name}%"));
            }

            if (int.TryParse(productNumber, out var parsedProductNumber))
            {
                query = query.Where(p => p.Number == parsedProductNumber);
            }

            return await query.ToListAsync();
        }

        public async Task<Product?> GetByProductNumberAsync(int productNumber)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Number == productNumber);
        }


    }
}
