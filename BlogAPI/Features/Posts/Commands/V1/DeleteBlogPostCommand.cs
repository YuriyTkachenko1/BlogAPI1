using MediatR;

namespace BlogAPI.Features.Posts.Commands.V1
{
    public record DeleteBlogPostCommand(int Id) : IRequest<bool>;
}
