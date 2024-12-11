using BlogPostWebAPI.Models;
using BlogPostWebAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BlogPostWebAPI.Controllers
{
    /// <summary>
    /// Controller for blogs.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BlogController : ControllerBase
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<BlogController> _logger;

        /// <summary>
        /// The blog repository
        /// </summary>
        private readonly IBlogRepository _blogRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="blogRepository">The blog repository.</param>
        public BlogController(ILogger<BlogController> logger, IBlogRepository blogRepository)
        {
            _logger = logger;
            _blogRepository = blogRepository;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var blogs = await _blogRepository.GetAllAsync();
            return Ok(blogs);
        }

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var blog = await _blogRepository.GetByIdAsync(id);
            if (blog == null) return NotFound(new { message = "Blog not found" });
            return Ok(blog);
        }

        /// <summary>
        /// Creates the specified blog.
        /// </summary>
        /// <param name="blog">The blog.</param>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BlogPost blog)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            blog.DateCreated = DateTime.UtcNow;
            await _blogRepository.AddAsync(blog);
            return CreatedAtAction(nameof(GetById), new { id = blog.Id }, blog);
        }

        /// <summary>
        /// Updates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="blog">The blog.</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BlogPost blog)
        {
            if (id != blog.Id) return BadRequest(new { message = "ID mismatch" });
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                await _blogRepository.UpdateAsync(blog);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _blogRepository.DeleteAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
