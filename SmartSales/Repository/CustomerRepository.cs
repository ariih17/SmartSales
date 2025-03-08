using SmartSales.Data;
using SmartSales.Models;
using SmartSales.Repository.IRepository;

namespace SmartSales.Repository
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        private ApplicationDbContext _db;
        public CustomerRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Customer obj)
        {
            _db.COM_CUSTOMER.Update(obj);
        }
    }
}
