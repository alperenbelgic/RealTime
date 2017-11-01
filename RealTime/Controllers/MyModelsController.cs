using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealTime.Models;

namespace RealTime.Controllers
{
    [Produces("application/json")]
    [Route("api/MyModels")]
    public class MyModelsController : Controller
    {
        private readonly RealTimeContext _context;

        public MyModelsController(RealTimeContext context)
        {
            _context = context;
        }

        // GET: api/MyModels
        [HttpGet]
        public IEnumerable<MyModel> GetMyModel()
        {
            return _context.MyModel;
        }

        // GET: api/MyModels/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMyModel([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var myModel = await _context.MyModel.SingleOrDefaultAsync(m => m.Id == id);

            if (myModel == null)
            {
                return NotFound();
            }

            return Ok(myModel);
        }

        // PUT: api/MyModels/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMyModel([FromRoute] int id, [FromBody] MyModel myModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != myModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(myModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MyModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/MyModels
        [HttpPost]
        public async Task<IActionResult> PostMyModel([FromBody] MyModel myModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.MyModel.Add(myModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMyModel", new { id = myModel.Id }, myModel);
        }

        // DELETE: api/MyModels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMyModel([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var myModel = await _context.MyModel.SingleOrDefaultAsync(m => m.Id == id);
            if (myModel == null)
            {
                return NotFound();
            }

            _context.MyModel.Remove(myModel);
            await _context.SaveChangesAsync();

            return Ok(myModel);
        }

        private bool MyModelExists(int id)
        {
            return _context.MyModel.Any(e => e.Id == id);
        }
    }
}