using BlogAPI.Models;
using BlogAPI.Features.Posts;
using BlogAPI.Services;
using MediatR;
using Microsoft.Extensions.Hosting;
using BlogAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Services
{
    // Renamed the class to avoid conflict with the existing 'BlogPostService' class in the same namespace.  
    public class BlogPostService : IBlogPostService
    {
        private readonly BlogDbContext _context;

        public BlogPostService(BlogDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IDExistsAsync(int Id)
        {
            return await _context.BlogPosts.AnyAsync(p => p.Id == Id);
        }
    }
}
