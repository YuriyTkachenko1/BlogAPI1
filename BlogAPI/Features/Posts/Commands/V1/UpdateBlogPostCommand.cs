using BlogAPI.Dtos.V1;
using MediatR;

namespace BlogAPI.Features.Posts.Commands.V1
{
    public record UpdateBlogPostCommand(int Id, BlogPostDto Dto) : IRequest<bool>;
}
