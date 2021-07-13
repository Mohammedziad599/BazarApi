using System.Collections.Generic;

using BazarOrderApi.Models;

namespace BazarOrderApi.Data
{
    public interface IOrderRepo
    {
        public IEnumerable<Order> GetAllOrders();
        public Order GetOrderById(int id);
        public void AddOrder(Order order);
        public bool SaveChanges();
    }
}