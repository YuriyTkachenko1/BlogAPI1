using BlogAPI.Dtos;
using BlogAPI.Features.Comments;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CommentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CommentDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var id = await _mediator.Send(new CreateCommentCommand(dto));
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CommentDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _mediator.Send(new UpdateCommentCommand(id, dto));
            return result ? NoContent() : NotFound();
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var comment = await _mediator.Send(new GetCommentByIdQuery(id));
            return comment == null ? NotFound() : Ok(comment);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _mediator.Send(new GetAllCommentsQuery());
            return Ok(comments);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteCommentCommand(id));
            return result ? NoContent() : NotFound();
        }
    }
}
