using MicroBlog.Models;
using MicroBlog.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MicroBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IDataContext _context;
        private readonly IDataRepository<Blog> _repo;

        public BlogController(IDataContext context, IDataRepository<Blog> repo)
        {
            _context = context;
            _repo = repo;
        }

        [HttpPost]
        [Route("PostBlog")]
        public async Task<IActionResult> PostBlog([FromBody] Blog blog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _repo.Add(blog);
            await _repo.SaveAsync(blog);

            return CreatedAtAction("GetAuthor", new { id = blog.BlogId }, blog);
        }

        [HttpGet]
        [Route("GetBlogs")]
        public IActionResult GetBlogs()
        {
            IEnumerable<Blog> blogs = _context.Blogs.OrderByDescending(p => p.PublishedDate);
            return Ok(blogs);
        }

        [HttpGet]
        [Route("GetBlogs/{id}")]
        public IEnumerable<Blog> GetBlogs([FromRoute] int id)
        {
            return _context.Blogs.Where(a => a.AuthorId == id).OrderByDescending(p => p.PublishedDate);
        }

        [HttpPut]
        [Route("UpdateBlog/{id}")]
        public async Task<IActionResult> UpdateBlog([FromRoute] int id, [FromBody] Blog blog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != blog.BlogId)
            {
                return BadRequest();
            }

            _context.SetModified(blog);

            try
            {
                _repo.Update(blog);
                await _repo.SaveAsync(blog);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Blogs.Any(e => e.BlogId == id))
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
        [Route("DeleteBlog/{id}")]
        public async Task<IActionResult> DeleteBlog([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }

            _repo.Delete(blog);
            await _repo.SaveAsync(blog);

            return Ok(blog);
        }
    }
}
