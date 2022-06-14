using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int Id)
        {
            var result = _context.CelestialObjects.FirstOrDefault(e => e.OrbitedObjectId == Id);

            if(result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }
        [HttpGet("name")]
        public IActionResult GetByName(string name)
        {
            var result = _context.CelestialObjects.FirstOrDefault(e => e.Name == name);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.CelestialObjects.ToList());
        }
    }
}
