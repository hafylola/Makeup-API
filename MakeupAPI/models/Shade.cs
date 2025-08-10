namespace MakeupAPI.models
{
    public class Shade
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string HexCode { get; set; }

        public int productId { get; set; }
        public Product Product { get; set; }
    }
}
