using BlogAPI.Dtos.V2;
using MediatR;

namespace BlogAPI.Features.Posts
{
    public record GetAllBlogPostsQuery() : IRequest<List<BlogPostResponseDto>>;
}
