using BazarOrderApi.Models;

namespace BazarOrderApi.Data
{
    public class SqlOrderRepo : IOrderRepo
    {
        private readonly OrderDbContext _context;

        public SqlOrderRepo(OrderDbContext context)
        {
            _context = context;
        }

        public void AddOrder(Order order)
        {
            _context.Add(order);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}