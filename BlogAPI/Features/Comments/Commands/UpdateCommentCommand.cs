using BlogAPI.Dtos.V1;
using MediatR;

namespace BlogAPI.Features.Comments
{
    public record UpdateCommentCommand(int Id, CommentDto Dto) : IRequest<bool>;
}
