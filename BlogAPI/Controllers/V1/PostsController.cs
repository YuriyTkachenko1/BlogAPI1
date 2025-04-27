using Microsoft.AspNetCore.Mvc;
using MediatR;
using BlogAPI.Features.Posts;
using BlogAPI.Dtos.V1;
using BlogAPI.Features.Posts.Commands.V1;
using BlogAPI.Features.Posts.Queries.V1;
using Asp.Versioning;

namespace BlogAPI.Controllers.V1
{
    [ApiController]
    [ApiVersion(1)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PostsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// Create new blog.
        /// </summary>
        /// <returns>Status Code</returns>
        [HttpPost]
        [MapToApiVersion(1)]
        public async Task<IActionResult> CreatePost([FromBody] BlogPostDto Dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = await _mediator.Send(new CreateBlogPostCommand(Dto));
            return CreatedAtAction(nameof(GetPostById), new { id }, null);
        }
        /// <summary>
        /// Update the blog by ID.
        /// </summary>
        /// <returns>Status Code</returns>
        [HttpPut("{id:int}")]
        [MapToApiVersion(1)]
        public async Task<IActionResult> Update(int id, [FromBody] BlogPostDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateBlogPostCommand(id, dto));
            if (!result)
                return NotFound();
            return NoContent();
        }
        /// <summary>
        /// Gets a blog by ID.
        /// </summary>
        /// <returns>Status Code</returns>
        [HttpGet("{id:int}")]
        [MapToApiVersion(1)]
        public async Task<IActionResult> GetPostById(int id)
        {
            var post = await _mediator.Send(new GetBlogPostByIdQuery(id));
            if (post == null)
                return NotFound();
            return Ok(post);
        }

        /// <summary>
        /// Gets the list of blogs.
        /// </summary>
        /// <returns>List of blogs</returns>
        [HttpGet]
        [MapToApiVersion(1)]
        public async Task<IActionResult> Get()
        {
            var posts = await _mediator.Send(new GetAllBlogPostsQuery());
            return Ok(posts);
        }

        /// <summary>
        /// Deletes blog by ID.
        /// </summary>
        /// <returns>Status Code</returns>
        [HttpDelete("{id:int}")]
        [MapToApiVersion(1)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteBlogPostCommand(id));
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
}