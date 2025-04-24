using BlogAPI.Models;
using MediatR;

namespace BlogAPI.Features.Comments
{
    public record GetAllCommentsQuery() : IRequest<List<Comment>>;
}
