namespace BazarOrderApi.Models
{
    public class Book
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Topic { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }
    }
}