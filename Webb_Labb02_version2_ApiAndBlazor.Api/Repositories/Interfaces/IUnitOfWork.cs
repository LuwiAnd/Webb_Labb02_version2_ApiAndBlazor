using System.Threading.Tasks;

namespace Webb_Labb02_version2_ApiAndBlazor.Api.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IProductRepository Products { get; }
        IOrderRepository Orders { get; }
        IOrderItemRepository OrderItems { get; }

        Task<int> CompleteAsync();
    }
}
