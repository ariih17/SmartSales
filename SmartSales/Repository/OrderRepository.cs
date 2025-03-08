using SmartSales.Data;
using SmartSales.Models;
using SmartSales.Repository.IRepository;

namespace SmartSales.Repository
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private ApplicationDbContext _db;
        public OrderRepository(ApplicationDbContext db) : base(db) 
        {
            _db = db;   
        }
        public void Update(Order obj)
        {
            _db.SO_ORDER.Update(obj);
        }
    }
}
