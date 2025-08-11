using MakeupAPI.Data;
using MakeupAPI.models;
using MakeupAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeupAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly MakeupContext _db;
        public ProductsController(MakeupContext db) => _db = db;

        // ---- DTOs (no cycles) ----
        public record BrandDto(int Id, string Name);
        public record CategoryDto(int Id, string Name);
        public record ShadeDto(int Id, string Name);
        public record ProductDto(int Id, string Name, decimal Price, BrandDto Brand, CategoryDto Category, List<ShadeDto> Shades);

        public record CreateShadeDto(string Name);
        public record CreateProductDto(string Name, int BrandId, int CategoryId, decimal Price, List<CreateShadeDto> Shades);

        private static ProductDto ToDto(Product p) =>
            new ProductDto(
                p.Id,
                p.Name,
                p.Price,
                new BrandDto(p.BrandId, p.Brand?.Name ?? ""),
                new CategoryDto(p.CategoryId, p.Category?.Name ?? ""),
                (p.Shades ?? new List<Shade>()).Select(s => new ShadeDto(s.Id, s.Name)).ToList()
            );

        // GET /api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll()
        {
            var items = await _db.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Shades)
                .AsNoTracking()
                .ToListAsync();

            return Ok(items.Select(ToDto));
        }

        // GET /api/products/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetOne(int id)
        {
            var p = await _db.Products
                .Include(x => x.Brand)
                .Include(x => x.Category)
                .Include(x => x.Shades)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (p is null) return NotFound();
            return Ok(ToDto(p));
        }

        // POST /api/products
        [HttpPost]
        public async Task<ActionResult<ProductDto>> Create([FromBody] CreateProductDto body)
        {
            if (string.IsNullOrWhiteSpace(body.Name))
                return BadRequest("Name is required.");

            // (Optional) verify FKs exist
            var brandExists = await _db.Brands.AnyAsync(b => b.Id == body.BrandId);
            var catExists = await _db.Categories.AnyAsync(c => c.Id == body.CategoryId);
            if (!brandExists || !catExists) return BadRequest("Invalid brandId or categoryId.");

            var product = new Product
            {
                Name = body.Name.Trim(),
                BrandId = body.BrandId,
                CategoryId = body.CategoryId,
                Price = body.Price,
                Shades = (body.Shades ?? new List<CreateShadeDto>())
                    .Where(s => !string.IsNullOrWhiteSpace(s.Name))
                    .Select(s => new Shade { Name = s.Name.Trim() })
                    .ToList()
            };

            _db.Products.Add(product);
            await _db.SaveChangesAsync();

            // load navs for DTO
            await _db.Entry(product).Reference(p => p.Brand).LoadAsync();
            await _db.Entry(product).Reference(p => p.Category).LoadAsync();
            await _db.Entry(product).Collection(p => p.Shades).LoadAsync();

            return CreatedAtAction(nameof(GetOne), new { id = product.Id }, ToDto(product));
        }

        // DELETE /api/products/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var p = await _db.Products.Include(x => x.Shades).FirstOrDefaultAsync(x => x.Id == id);
            if (p is null) return NotFound();

            // If cascade delete isn’t configured, remove shades first:
            if (p.Shades?.Count > 0) _db.Shades.RemoveRange(p.Shades);

            _db.Products.Remove(p);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
