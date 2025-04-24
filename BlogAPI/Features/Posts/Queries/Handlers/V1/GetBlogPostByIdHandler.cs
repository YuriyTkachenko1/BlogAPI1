using BlogAPI.Models;
using BlogAPI.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using BlogAPI.Features.Posts.Queries.V1;

namespace BlogAPI.Features.Posts.Queries.Handlers.V1
{
    public class GetBlogPostByIdHandler : IRequestHandler<GetBlogPostByIdQuery, BlogPost?>
    {
        private readonly BlogDbContext _context;

        public GetBlogPostByIdHandler(BlogDbContext context)
        {
            _context = context;
        }

        public async Task<BlogPost?> Handle(GetBlogPostByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.BlogPosts
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        }
    }
}
