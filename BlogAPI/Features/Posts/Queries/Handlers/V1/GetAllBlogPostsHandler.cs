using BlogAPI.Data;
using BlogAPI.Features.Posts.Queries.V1;
using BlogAPI.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Features.Posts.Queries.Handlers.V1
{
    public class GetAllBlogPostsHandler : IRequestHandler<GetAllBlogPostsQuery, List<BlogPost>>
    {
        private readonly BlogDbContext _context;

        public GetAllBlogPostsHandler(BlogDbContext context)
        {
            _context = context;
        }

        public async Task<List<BlogPost>> Handle(GetAllBlogPostsQuery request, CancellationToken cancellationToken)
        {
            return await _context.BlogPosts.Include(p => p.Comments).ToListAsync(cancellationToken);
        }
    }
}
