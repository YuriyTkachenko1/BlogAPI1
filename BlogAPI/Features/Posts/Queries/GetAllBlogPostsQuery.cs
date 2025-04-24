using BlogAPI.Models;
using MediatR;

namespace BlogAPI.Features.Posts
{
    public record GetAllBlogPostsQuery() : IRequest<List<BlogPost>>;
}
