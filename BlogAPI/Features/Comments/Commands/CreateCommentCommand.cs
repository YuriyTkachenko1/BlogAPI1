using BlogAPI.Dtos;
using MediatR;

namespace BlogAPI.Features.Comments
{
    public record CreateCommentCommand(CommentDto Dto) : IRequest<int>;
}
