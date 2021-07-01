namespace BazarCatalogApi.Dtos
{
    public class BookReadDto
    {
        public string Name { get; set; }

        public string Topic { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }
    }
}