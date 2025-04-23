using BlogAPI.Models;
using BlogAPI.Data;
using MediatR;
using Microsoft.Extensions.Hosting;

namespace BlogAPI.Features.Posts
{
    public class CreateBlogPostHandler : IRequestHandler<CreateBlogPostCommand, int>
    {
        private readonly BlogDbContext _context;

        public CreateBlogPostHandler(BlogDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateBlogPostCommand request, CancellationToken cancellationToken)
        {
            var post = new BlogPost
            {
                Title = request.Title,
                Content = request.Content,
                CreatedAt = DateTime.UtcNow
            };

            await _context.BlogPosts.AddAsync(post, cancellationToken);

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {

                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
            return post.Id;
        }
    }
}
