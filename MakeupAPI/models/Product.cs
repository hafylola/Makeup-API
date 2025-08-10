using MakeupAPI.Models;

namespace MakeupAPI.models
{
    public class Product
    {
        public int Id { get; set;}
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }

        public int Id { get; set; }
        public Brand Brand { get; set; }

        public int BrandId { get; set; }
        public Category Category { get; set; }

        public ICollection<Shade> Shades { get; set; }


    }
}
