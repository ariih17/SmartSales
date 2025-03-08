using SmartSales.Data;
using SmartSales.Repository.IRepository;

namespace SmartSales.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;

        public IOrderRepository OrderRepository { get; set; }
        public ICustomerRepository CustomerRepository { get; set; }
        public IItemRepository ItemRepository { get; set; }
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            OrderRepository = new OrderRepository(_db);
            CustomerRepository = new CustomerRepository(_db);
            ItemRepository = new ItemRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
