using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

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

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int Id)
        {
            var celestialObject = _context.CelestialObjects.Find(Id);

            if(celestialObject == null) 
                return NotFound();

            celestialObject.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == Id).ToList();
            return Ok(celestialObject);
        }
        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObjects = _context.CelestialObjects.Where(e => e.Name == name).ToList();
            if (!celestialObjects.Any())  
                return NotFound();

            foreach (var celestialObject in celestialObjects)
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == celestialObject.OrbitedObjectId).ToList();
            }
            return Ok(celestialObjects);
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var celestialObjects = _context.CelestialObjects.ToList();
            foreach (var celestialObject in celestialObjects)
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == celestialObject.OrbitedObjectId).ToList();
            }
            return Ok(celestialObjects);
        }

        [HttpPost]
        public IActionResult Create([FromBody]CelestialObject model)
        {
            _context.CelestialObjects.Add(model);
            _context.SaveChanges();
            return CreatedAtRoute("GetById", new { id = model.Id }, model);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int Id, CelestialObject model)
        {
            var celestialObjects = _context.CelestialObjects.Find(Id);
            if (celestialObjects == null)
                return NotFound();

            celestialObjects.Name = model.Name;
            celestialObjects.OrbitalPeriod = model.OrbitalPeriod;
            celestialObjects.OrbitedObjectId = model.OrbitedObjectId;
            _context.Update(celestialObjects);
            _context.SaveChanges();
            return NoContent();
        }
        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var celestialObjects = _context.CelestialObjects.Find(id);
            if (celestialObjects == null)
                return NotFound();
            celestialObjects.Name = name;

            _context.Update(celestialObjects);
            _context.SaveChanges();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var celestialObjects = _context.CelestialObjects.Find(id);
            if (celestialObjects == null)
                return NotFound();
            _context.RemoveRange(celestialObjects);
            _context.SaveChanges();
            return NoContent();
        }
    }

}
