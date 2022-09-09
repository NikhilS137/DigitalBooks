using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Models;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class CategoryMastersController : ControllerBase
    {
        private readonly DBDigitalBooksContext _context;

        public CategoryMastersController(DBDigitalBooksContext context)
        {
            _context = context;
        }

        // GET: api/CategoryMasters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryMaster>>> GetCategoryMasters()
        {
          if (_context.CategoryMasters == null)
          {
              return NotFound();
          }
            return await _context.CategoryMasters.ToListAsync();
        }

        // GET: api/CategoryMasters/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryMaster>> GetCategoryMaster(int id)
        {
          if (_context.CategoryMasters == null)
          {
              return NotFound();
          }
            var categoryMaster = await _context.CategoryMasters.FindAsync(id);

            if (categoryMaster == null)
            {
                return NotFound();
            }

            return categoryMaster;
        }

        // PUT: api/CategoryMasters/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoryMaster(int id, CategoryMaster categoryMaster)
        {
            if (id != categoryMaster.CategoryId)
            {
                return BadRequest();
            }

            _context.Entry(categoryMaster).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryMasterExists(id))
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

        // POST: api/CategoryMasters
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CategoryMaster>> PostCategoryMaster(CategoryMaster categoryMaster)
        {
          if (_context.CategoryMasters == null)
          {
              return Problem("Entity set 'DBDigitalBooksContext.CategoryMasters'  is null.");
          }
            _context.CategoryMasters.Add(categoryMaster);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategoryMaster", new { id = categoryMaster.CategoryId }, categoryMaster);
        }

        // DELETE: api/CategoryMasters/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoryMaster(int id)
        {
            if (_context.CategoryMasters == null)
            {
                return NotFound();
            }
            var categoryMaster = await _context.CategoryMasters.FindAsync(id);
            if (categoryMaster == null)
            {
                return NotFound();
            }

            _context.CategoryMasters.Remove(categoryMaster);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoryMasterExists(int id)
        {
            return (_context.CategoryMasters?.Any(e => e.CategoryId == id)).GetValueOrDefault();
        }
    }
}
