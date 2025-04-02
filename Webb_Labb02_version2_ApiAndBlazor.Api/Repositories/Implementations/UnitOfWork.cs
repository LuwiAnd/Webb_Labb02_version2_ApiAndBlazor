using Webb_Labb02_version2_ApiAndBlazor.Api.Data;
using Webb_Labb02_version2_ApiAndBlazor.Api.Repositories.Interfaces;

namespace Webb_Labb02_version2_ApiAndBlazor.Api.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IUserRepository Users { get; }

        public IProductRepository Products { get; }

        public IOrderRepository Orders { get; }

        public IOrderItemRepository OrderItems { get; }

        public UnitOfWork(
            ApplicationDbContext context,
            IUserRepository users,
            IProductRepository products,
            IOrderRepository orders,
            IOrderItemRepository orderItems
            )
        {
            _context = context;
            Users = users;
            Products = products;
            Orders = orders;
            OrderItems = orderItems;
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
