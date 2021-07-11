using BazarOrderApi.Models;

namespace BazarOrderApi.Data
{
    public interface IOrderRepo
    {
        public void AddOrder(Order order);
        public bool SaveChanges();
    }
}