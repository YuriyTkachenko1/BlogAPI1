using Asp.Versioning;
using BlogAPI.Dtos.V1;
using BlogAPI.Features.Comments;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Controllers.V1
{
    [ApiController]
    [ApiVersion(1)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CommentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [MapToApiVersion(1)]
        public async Task<IActionResult> Create([FromBody] CommentDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var id = await _mediator.Send(new CreateCommentCommand(dto));
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpGet("{id:int}")]
        [MapToApiVersion(1)]
        public async Task<IActionResult> GetById(int id)
        {
            var comment = await _mediator.Send(new GetCommentByIdQuery(id));
            return comment == null ? NotFound() : Ok(comment);
        }

        [HttpGet]
        [MapToApiVersion(1)]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _mediator.Send(new GetAllCommentsQuery());
            return Ok(comments);
        }

        [HttpGet("by-post/{postId:int}")]
        [MapToApiVersion(1)]
        public async Task<IActionResult> GetByBlogPostId(int postId)
        {
            var comments = await _mediator.Send(new GetCommentsByBlogPostIdQuery(postId));
            return Ok(comments);
        }
    }
}
