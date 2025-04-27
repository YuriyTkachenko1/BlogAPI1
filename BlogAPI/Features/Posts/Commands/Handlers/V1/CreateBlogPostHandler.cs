using BlogAPI.Models;
using BlogAPI.Data;
using MediatR;
using Microsoft.Extensions.Hosting;
using BlogAPI.Features.Posts.Commands.V1;

namespace BlogAPI.Features.Posts.Commands.Handlers.V1
{
    public class CreateBlogPostHandler : IRequestHandler<CreateBlogPostCommand, int>
    {
        private readonly BlogDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateBlogPostHandler(BlogDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<int> Handle(CreateBlogPostCommand request, CancellationToken cancellationToken)
        {
            var username = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "UnknownUser";
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var post = new BlogPost
                {
                    Title = request.Dto.Title,
                    Content = request.Dto.Content,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = username
                };

                await _context.BlogPosts.AddAsync(post, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync();
                return post.Id;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
