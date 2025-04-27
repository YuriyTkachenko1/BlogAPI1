using BlogAPI.Dtos.V1;
using MediatR;

namespace BlogAPI.Features.Posts
{
    public record GetAllBlogPostsQuery() : IRequest<List<BlogPostResponseDto>>;
}
