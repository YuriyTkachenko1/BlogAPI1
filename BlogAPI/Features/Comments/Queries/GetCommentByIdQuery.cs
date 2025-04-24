using BlogAPI.Models;
using MediatR;

namespace BlogAPI.Features.Comments
{
    public record GetCommentByIdQuery(int Id) : IRequest<Comment?>;
}
