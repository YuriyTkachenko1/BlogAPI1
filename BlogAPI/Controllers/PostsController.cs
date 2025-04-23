using Microsoft.AspNetCore.Mvc;
using MediatR;
using BlogAPI.Features.Posts;

namespace BlogApi.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class PostsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PostsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] CreateBlogPostCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetPostById), new { id }, null);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetPostById(int id)
        {
            var post = await _mediator.Send(new GetBlogPostByIdQuery(id));
            if (post == null)
                return NotFound();
            return Ok(post);
        }
    }
}