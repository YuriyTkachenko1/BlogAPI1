using BlogAPI.Models;
using BlogAPI.Features.Posts;
using BlogAPI.Services;
using MediatR;
using Microsoft.Extensions.Hosting;

namespace BlogAPI.Services
{
    public class BlogService : IBlogService
    {
        private readonly IMediator _mediator;

        public BlogService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<int> CreatePostAsync(string title, string content)
        {
            return await _mediator.Send(new CreateBlogPostCommand(title, content));
        }

        public async Task<BlogPost?> GetPostAsync(int id)
        {
            return await _mediator.Send(new GetBlogPostByIdQuery(id));
        }
    }
}
