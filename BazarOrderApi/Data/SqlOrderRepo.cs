using System.Collections.Generic;
using System.Linq;

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

        public IEnumerable<Order> GetAllOrders()
        {
            var orders = _context.Orders.ToList();
            return !orders.Any() ? null : orders;
        }

        public Order GetOrderById(int id)
        {
            var order = _context.Orders.FirstOrDefault(_order => _order.Id == id);
            return order;
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