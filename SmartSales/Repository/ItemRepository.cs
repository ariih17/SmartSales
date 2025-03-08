using Microsoft.EntityFrameworkCore;
using SmartSales.Data;
using SmartSales.Models;
using SmartSales.Repository.IRepository;

namespace SmartSales.Repository
{
    public class ItemRepository : Repository<Item>, IItemRepository
    {
        private ApplicationDbContext _db;
        public ItemRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void AddRange(List<Item> getAllItems)
        {
            _db.AddRange(getAllItems);
        }

        public void Detach(Item existingItem)
        {
            var entry = _db.Entry(existingItem);
            if (entry.State == EntityState.Detached)
            {
                return;
            }
            entry.State = EntityState.Detached;
        }
        public void DetachAll()
        {
            foreach (var entity in _db.ChangeTracker.Entries())
            {
                entity.State = EntityState.Detached; // Menanggalkan entitas dari konteks
            }
        }

        //public IQueryable<Item> GetAll()
        //{
        //    return _db.SO_ITEM.AsNoTracking();
        //}

        public void Update(Item obj)
        {
            _db.SO_ITEM.Update(obj);
        }
    }
}
