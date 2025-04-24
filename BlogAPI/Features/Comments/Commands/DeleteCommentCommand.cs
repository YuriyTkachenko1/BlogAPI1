using MediatR;

namespace BlogAPI.Features.Comments
{
    public record DeleteCommentCommand(int Id) : IRequest<bool>;
}
