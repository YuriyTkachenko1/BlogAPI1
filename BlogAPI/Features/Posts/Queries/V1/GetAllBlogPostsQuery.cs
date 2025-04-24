using BlogAPI.Models;
using MediatR;

namespace BlogAPI.Features.Posts.Queries.V1
{
    public record GetAllBlogPostsQuery() : IRequest<List<BlogPost>>;
}
