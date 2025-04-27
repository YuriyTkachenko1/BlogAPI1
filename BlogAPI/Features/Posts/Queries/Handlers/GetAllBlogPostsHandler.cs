using BlogAPI.Data;
using BlogAPI.Dtos.V1;
using MediatR;
using Microsoft.EntityFrameworkCore;
using BlogAPI.Services;

namespace BlogAPI.Features.Posts
{
    public class GetAllBlogPostsHandler : IRequestHandler<GetAllBlogPostsQuery, List<BlogPostResponseDto>>
    {
        private readonly BlogDbContext _context;
        private readonly IAuditService _auditService;
        private readonly ILogger<GetAllBlogPostsHandler> _logger;

        public GetAllBlogPostsHandler(BlogDbContext context, IAuditService auditService, ILogger<GetAllBlogPostsHandler> logger)
        {
            _context = context;
            _auditService = auditService;
            _logger = logger;
        }

        public async Task<List<BlogPostResponseDto>> Handle(GetAllBlogPostsQuery request, CancellationToken cancellationToken)
        {
            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                var posts = await _context.BlogPosts
                    .Include(p => p.Comments)
                    .Select(p => new BlogPostResponseDto
                    {
                        Id = p.Id,
                        Title = p.Title,
                        Content = p.Content,
                        CreatedAt = p.CreatedAt,
                        Comments = p.Comments
                    })
                    .ToListAsync(cancellationToken);

                await _auditService.LogAsync("Fetched All BlogPosts", 0);
                await transaction.CommitAsync(cancellationToken);

                _logger.LogInformation("Fetched {Count} blog posts", posts.Count);
                return posts;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.LogError(ex, "Error fetching all blog posts");
                throw;
            }
        }
    }
}
