namespace SmartSales.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IOrderRepository OrderRepository { get; }
        ICustomerRepository CustomerRepository { get; }
        IItemRepository ItemRepository { get; }

        void Save();
    }
}
