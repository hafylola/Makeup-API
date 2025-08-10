using Microsoft.AspNetCore.Mvc;
using MakeupAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace MakeupAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private static List<Brand> _brands = new List<Brand>();

        // GET: api/brand
        [HttpGet]
        public ActionResult<IEnumerable<Brand>> GetBrands()
        {
            return Ok(_brands);
        }

        // GET: api/brand/{id}
        [HttpGet("{id}")]
        public ActionResult<Brand> GetBrand(int id)
        {
            var brand = _brands.FirstOrDefault(b => b.Id == id);
            if (brand == null)
                return NotFound();
            return Ok(brand);
        }

        // POST: api/brand
        [HttpPost]
        public ActionResult<Brand> CreateBrand(Brand brand)
        {
            brand.Id = _brands.Count + 1;
            _brands.Add(brand);
            return CreatedAtAction(nameof(GetBrand), new { id = brand.Id }, brand);
        }

        // PUT: api/brand/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateBrand(int id, Brand updatedBrand)
        {
            var brand = _brands.FirstOrDefault(b => b.Id == id);
            if (brand == null)
                return NotFound();

            brand.Name = updatedBrand.Name;
            return NoContent();
        }

        // DELETE: api/brand/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteBrand(int id)
        {
            var brand = _brands.FirstOrDefault(b => b.Id == id);
            if (brand == null)
                return NotFound();

            _brands.Remove(brand);
            return NoContent();
        }
    }
}

