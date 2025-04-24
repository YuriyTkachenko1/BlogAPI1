using BlogAPI.Dtos;
using MediatR;

namespace BlogAPI.Features.Comments
{
    public record UpdateCommentCommand(int Id, CommentDto Dto) : IRequest<bool>;
}
