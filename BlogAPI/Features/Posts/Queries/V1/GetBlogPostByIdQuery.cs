using BlogAPI.Models;
using MediatR;
using Microsoft.Extensions.Hosting;

namespace BlogAPI.Features.Posts.Queries.V1
{
    public record GetBlogPostByIdQuery(int Id) : IRequest<BlogPost?>;

}
