using MakeupAPI.models;
using MakeupAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace MakeupAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShadeController : ControllerBase
    {
        private static List<Shade> _shades = new List<Shade>();

        // GET: api/shade
        [HttpGet]
        public ActionResult<IEnumerable<Shade>> GetShades()
        {
            return Ok(_shades);
        }

        // GET: api/shade/{id}
        [HttpGet("{id}")]
        public ActionResult<Shade> GetShade(int id)
        {
            var shade = _shades.FirstOrDefault(s => s.Id == id);
            if (shade == null)
                return NotFound();
            return Ok(shade);
        }

        // POST: api/shade
        [HttpPost]
        public ActionResult<Shade> CreateShade(Shade shade)
        {
            shade.Id = _shades.Count + 1;
            _shades.Add(shade);
            return CreatedAtAction(nameof(GetShade), new { id = shade.Id }, shade);
        }

        // PUT: api/shade/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateShade(int id, Shade updatedShade)
        {
            var shade = _shades.FirstOrDefault(s => s.Id == id);
            if (shade == null)
                return NotFound();

            shade.Name = updatedShade.Name;
            shade.HexCode = updatedShade.HexCode;
            return NoContent();
        }

        // DELETE: api/shade/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteShade(int id)
        {
            var shade = _shades.FirstOrDefault(s => s.Id == id);
            if (shade == null)
                return NotFound();

            _shades.Remove(shade);
            return NoContent();
        }
    }
}
