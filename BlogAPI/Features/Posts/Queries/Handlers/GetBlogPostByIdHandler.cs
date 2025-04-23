using BlogAPI.Models;
using BlogAPI.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace BlogAPI.Features.Posts
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
