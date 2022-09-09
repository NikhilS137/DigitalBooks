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
    public class UserMastersController : ControllerBase
    {
        private readonly DBDigitalBooksContext _context;

        public UserMastersController(DBDigitalBooksContext context)
        {
            _context = context;
        }

        // GET: api/UserMasters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserMaster>>> GetUserMasters()
        {
          if (_context.UserMasters == null)
          {
              return NotFound();
          }
            return await _context.UserMasters.ToListAsync();
        }

        // GET: api/UserMasters/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserMaster>> GetUserMaster(int id)
        {
          if (_context.UserMasters == null)
          {
              return NotFound();
          }
            var userMaster = await _context.UserMasters.FindAsync(id);

            if (userMaster == null)
            {
                return NotFound();
            }

            return userMaster;
        }

        // PUT: api/UserMasters/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserMaster(int id, UserMaster userMaster)
        {
            if (id != userMaster.UserId)
            {
                return BadRequest();
            }
            userMaster.Password = EncryptionDecryption.EncodePasswordToBase64(userMaster.Password);

            _context.Entry(userMaster).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserMasterExists(id))
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

        // POST: api/UserMasters
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserMaster>> PostUserMaster(UserMaster userMaster)
        {
          if (_context.UserMasters == null)
          {
              return Problem("Entity set 'DBDigitalBooksContext.UserMasters'  is null.");
          }
            if (!UserNameExists(userMaster.UserName))
            {
                if (!EmailIDExists(userMaster.UserName))
                {
                    userMaster.Password = EncryptionDecryption.EncodePasswordToBase64(userMaster.Password);
                    _context.UserMasters.Add(userMaster);
                    await _context.SaveChangesAsync();
                }
                else
                    return Problem("Email ID already exits");
            }
            else
                return Problem("UserName already exits");

            return CreatedAtAction("GetUserMaster", new { id = userMaster.UserId }, userMaster);
        }

        // DELETE: api/UserMasters/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserMaster(int id)
        {
            if (_context.UserMasters == null)
            {
                return NotFound();
            }
            var userMaster = await _context.UserMasters.FindAsync(id);
            if (userMaster == null)
            {
                return NotFound();
            }

            _context.UserMasters.Remove(userMaster);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserNameExists(string userName)
        {
            return (_context.UserMasters?.Any(e => e.UserName == userName)).GetValueOrDefault();
        }
        private bool EmailIDExists(string EmailID)
        {
            return (_context.UserMasters?.Any(e => e.EmailId == EmailID)).GetValueOrDefault();
        }
        private bool UserMasterExists(int id)
        {
            return (_context.UserMasters?.Any(e => e.UserId == id)).GetValueOrDefault();
        }


        // GET: Get Author List
        [HttpGet]
        [Route("AuthorList")]
        public async Task<ActionResult<IEnumerable<UserMaster>>> AuthorList()
        {
            if (_context.UserMasters == null)
            {
                return NotFound();
            }
            return await _context.UserMasters.Where(u => u.RoleId == 1).ToListAsync();
        }

    }
}
