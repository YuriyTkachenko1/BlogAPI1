using BlogAPI.Dtos.V1;
using MediatR;

namespace BlogAPI.Features.Posts.Queries.V1
{
    public record GetAllBlogPostsQuery() : IRequest<List<BlogPostResponseDto>>;
}
