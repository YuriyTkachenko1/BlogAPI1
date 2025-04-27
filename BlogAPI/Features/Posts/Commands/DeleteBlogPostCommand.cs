using MediatR;

namespace BlogAPI.Features.Posts
{
    public record DeleteBlogPostCommand(int Id) : IRequest<bool>;
}
