using BlogAPI.Models;
using BlogAPI.Features.Posts;
using BlogAPI.Services;
using MediatR;
using Microsoft.Extensions.Hosting;
using BlogAPI.Dtos;

namespace BlogAPI.Services
{
    public class BlogService : IBlogService
    {
        private readonly IMediator _mediator;

        public BlogService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<int> CreatePostAsync(BlogPostDto Dto)
        {
            return await _mediator.Send(new CreateBlogPostCommand(Dto));
        }

        public async Task<BlogPost?> GetPostAsync(int id)
        {
            return await _mediator.Send(new GetBlogPostByIdQuery(id));
        }
    }
}
