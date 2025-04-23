using BlogAPI.Models;
using MediatR;
using Microsoft.Extensions.Hosting;

namespace BlogAPI.Features.Posts
{
    public record GetBlogPostByIdQuery(int Id) : IRequest<BlogPost?>;

}
