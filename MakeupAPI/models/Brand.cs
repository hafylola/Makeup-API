using MakeupAPI.models;

namespace MakeupAPI.Models
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }

        public ICollection<Product> Products { get; set; }

    }
}