using BazarOrderApi.Models;

namespace BazarOrderApi.Data
{
    public interface IOrderRepo
    {
        public Order AddOrder(Order order);
        public bool SaveChanges();
    }
}