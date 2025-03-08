using SmartSales.Models;

namespace SmartSales.Repository.IRepository
{
    public interface IItemRepository : IRepository<Item>
    {
        void AddRange(List<Item> getAllItems);
        void Detach(Item existingItem);
        void DetachAll();
        void Update(Item obj);
    }
}
