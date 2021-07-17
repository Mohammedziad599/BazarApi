namespace BazarCatalogApi.Dtos
{
    /// <summary>
    ///     this Dto (Data Transfer Object) will be used as the contract to send the data in
    ///     to the client because we don't want the used to know any thing other than those
    ///     this will become handy if we have updated the book Model to include a data that
    ///     we don't need to send to the user.
    /// </summary>
    public class BookReadDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Topic { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }
    }
}