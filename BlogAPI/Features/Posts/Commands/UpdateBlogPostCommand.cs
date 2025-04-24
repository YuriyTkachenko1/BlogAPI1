using BlogAPI.Dtos;
using MediatR;

namespace BlogAPI.Features.Posts
{
    public record UpdateBlogPostCommand(int Id, BlogPostDto Dto) : IRequest<bool>;
}
