using Asp.Versioning;
using BlogAPI.Dtos.V1;
using BlogAPI.Features.Comments;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Controllers
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

        /// <summary>
        /// Create new comment.
        /// </summary>
        /// <returns>New ID</returns>
        [HttpPost]
        [MapToApiVersion(1)]
        public async Task<IActionResult> Create([FromBody] CommentDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var id = await _mediator.Send(new CreateCommentCommand(dto));
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        /// <summary>
        /// Gets a comment by ID.
        /// </summary>
        /// <returns>The comment record</returns>
        [HttpGet("{id:int}")]
        [MapToApiVersion(1)]
        public async Task<IActionResult> GetById(int id)
        {
            var comment = await _mediator.Send(new GetCommentByIdQuery(id));
            return comment == null ? NotFound() : Ok(comment);
        }

        /// <summary>
        /// Gets the list of comments.
        /// </summary>
        /// <returns>List of comments</returns>
        [HttpGet]
        [MapToApiVersion(1)]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _mediator.Send(new GetAllCommentsQuery());
            return Ok(comments);
        }

        /// <summary>
        /// Gets the list of comments filtered by post ID.
        /// </summary>
        /// <returns>List of comments</returns>
        [HttpGet("by-post/{postId:int}")]
        [MapToApiVersion(1)]
        public async Task<IActionResult> GetByBlogPostId(int postId)
        {
            var comments = await _mediator.Send(new GetCommentsByBlogPostIdQuery(postId));
            return Ok(comments);
        }
    }
}
