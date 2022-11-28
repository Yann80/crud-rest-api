using MicroBlog.Models;
using MicroBlog.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MicroBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IDataRepository<Author> _repo;
        private readonly IDataContext _context;

        public AuthorController(IDataContext context, IDataRepository<Author> repo)
        {
            _repo = repo;
            _context = context;
        }

        [HttpPost]
        [Route("PostAuthor")]
        public async Task<IActionResult> PostAuthor([FromBody] Author author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _repo.Add(author);
            await _repo.SaveAsync(author);

            return CreatedAtAction("GetAuthor", new { id = author.AuthorId }, author);
        }

        [HttpGet]
        [Route("GetAuthors")]
        public IActionResult GetAuthors()
        {
            IEnumerable<Author> authors = _context.Authors.OrderByDescending(f => f.FirstName).ThenByDescending(l => l.LastName);
            return Ok(authors);
        }

        [HttpPut]
        [Route("UpdateAuthor/{id}")]
        public async Task<IActionResult> UpdateAuthor([FromRoute] int id, [FromBody] Author author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != author.AuthorId)
            {
                return BadRequest();
            }

            _context.SetModified(author);

            try
            {
                _repo.Update(author);
                await _repo.SaveAsync(author);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Authors.Any(e => e.AuthorId == id))
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

        [HttpDelete]
        [Route("DeleteAuthor/{id}")]
        public async Task<IActionResult> DeleteAuthor([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            _repo.Delete(author);
            await _repo.SaveAsync(author);

            return Ok(author);
        }
    }
}
