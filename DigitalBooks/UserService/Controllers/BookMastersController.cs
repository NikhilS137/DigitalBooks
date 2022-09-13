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
using UserService.ViewModels;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class BookMastersController : ControllerBase
    {
        private readonly DBDigitalBooksContext _context;

        public BookMastersController(DBDigitalBooksContext context)
        {
            _context = context;
        }

        // GET: api/BookMasters
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<BookMaster>>> GetBookMasters()
        {
            if (_context.BookMasters == null)
            {
                return NotFound();
            }
            return await _context.BookMasters.ToListAsync();
        }

        // GET: api/BookMasters/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<BookMaster>> GetBookMaster(int id)
        {
            if (_context.BookMasters == null)
            {
                return NotFound();
            }
            var bookMaster = await _context.BookMasters.FindAsync(id);

            if (bookMaster == null)
            {
                return NotFound();
            }

            return bookMaster;
        }

        // PUT: api/BookMasters/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutBookMaster(int id, BookMaster bookMaster)
        {
            if (id != bookMaster.BookId)
            {
                return BadRequest();
            }
            bookMaster.ModifiedDate = DateTime.Now;

            if (BookMasterWithAuthorExists(id, bookMaster.UserId))
                _context.Entry(bookMaster).State = EntityState.Modified;
            else
                return NotFound();

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookMasterExists(id))
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

        // POST: api/BookMasters
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<BookMaster>> PostBookMaster(BookMaster bookMaster)
        {
            if (_context.BookMasters == null)
            {
                return Problem("Entity set 'DBDigitalBooksContext.BookMasters'  is null.");
            }

            bookMaster.CreatedDate = DateTime.Now;

            _context.BookMasters.Add(bookMaster);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBookMaster", new { id = bookMaster.BookId }, bookMaster);
        }

        // DELETE: api/BookMasters/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteBookMaster(int id)
        {
            if (_context.BookMasters == null)
            {
                return NotFound();
            }
            var bookMaster = await _context.BookMasters.FindAsync(id);
            if (bookMaster == null)
            {
                return NotFound();
            }

            _context.BookMasters.Remove(bookMaster);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookMasterExists(int id)
        {
            return (_context.BookMasters?.Any(e => e.BookId == id)).GetValueOrDefault();
        }
        private bool BookMasterWithAuthorExists(int id, int userID)
        {
            return (_context.BookMasters?.Any(e => e.BookId == id && e.UserId == userID)).GetValueOrDefault();
        }

        [HttpGet]
        [Route("SearchBook")]
        public List<BookMasterViewModel> SearchBook(string title, int authorID, string publisher, DateTime publishedDate)
        {
            List<BookMasterViewModel> lsBookMaster = new List<BookMasterViewModel>();
            if (_context.BookMasters == null)
            {
                return lsBookMaster;
            }

            try
            {
                lsBookMaster = (from b in _context.BookMasters
                                join users in _context.UserMasters on b.UserId equals users.UserId
                                where b.BookName == title && b.UserId == authorID
                                && b.Publisher == publisher && b.PublishedDate.Date == publishedDate.Date
                                && b.Active == true
                                select new
                                {
                                    BookId = b.BookId,
                                    BookName = b.BookName,
                                    Author = users.FirstName + " " + users.LastName,
                                    Publisher = b.Publisher,
                                    Price = b.Price,
                                    PublishedDate = b.PublishedDate

                                }).ToList()
                                .Select(x => new BookMasterViewModel()
                                {
                                    BookId = x.BookId,
                                    Title = x.BookName,
                                    Author = x.Author,
                                    Publisher = x.Publisher,
                                    Price = Convert.ToDouble(x.Price),
                                    PublishedDate = x.PublishedDate
                                }).ToList();
            }
            catch (Exception ex)
            {
                return lsBookMaster;
            }

            return lsBookMaster;
        }

        [HttpGet]
        [Route("SearchBooks")]
        public List<BookMasterViewModel> SearchBooks(int categoryID,int authorID, decimal price )
        {
            List<BookMasterViewModel> lsBookMaster = new List<BookMasterViewModel>();
            if (_context.BookMasters == null)
            {
                return lsBookMaster;
            }

            try
            {
                lsBookMaster = (from b in _context.BookMasters
                                join users in _context.UserMasters on b.UserId equals users.UserId
                                join category in _context.CategoryMasters on b.CategoryId equals category.CategoryId
                                where b.CategoryId == categoryID || b.UserId == authorID
                                || b.Price == price
                                && b.Active == true
                                select new
                                {
                                    BookId = b.BookId,
                                    BookName = b.BookName,
                                    Author = users.FirstName + " " + users.LastName,
                                    Publisher = b.Publisher,
                                    Price = b.Price,
                                    PublishedDate = b.PublishedDate,
                                    CategoryName = category.CategoryName,
                                    Active = b.Active

                                }).ToList()
                                .Select(x => new BookMasterViewModel()
                                {
                                    BookId = x.BookId,
                                    Title = x.BookName,
                                    Author = x.Author,
                                    Publisher = x.Publisher,
                                    Price = Convert.ToDouble(x.Price),
                                    PublishedDate = x.PublishedDate,
                                    CategoryName = x.CategoryName,
                                    Active = x.Active
                                }).ToList();
            }
            catch (Exception ex)
            {
                return lsBookMaster;
            }

            return lsBookMaster;
        }
        [HttpPut("UpdateBookStatus/{BookId}/{UserID}/{Status}")]
        [Authorize]
        public async Task<IActionResult> UpdateBookStatus(int BookId, int UserID, bool Status)
        {
            if (BookId < 1)
            {
                return BadRequest();
            }

            if (BookMasterWithAuthorExists(BookId, UserID))
            {
                var book = _context.BookMasters.Find(BookId);
                book.Active = Status;
                book.ModifiedDate = DateTime.Now;
                _context.Entry(book).State = EntityState.Modified;
                //context.Entry(user).State = Entitystate.Modified;
            }               
            else
                return NotFound();

            try
            {
                _context.SaveChanges();
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookMasterExists(BookId))
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
    }
}
