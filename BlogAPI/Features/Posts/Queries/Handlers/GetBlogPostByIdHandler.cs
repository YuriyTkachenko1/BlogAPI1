using BlogAPI.Models;
using BlogAPI.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using BlogAPI.Dtos.V1;
using BlogAPI.Services;

namespace BlogAPI.Features.Posts
{

    public class GetBlogPostByIdHandler : IRequestHandler<GetBlogPostByIdQuery, BlogPostResponseDto?>
    {
        private readonly BlogDbContext _context;
        private readonly IAuditService _auditService;
        private readonly ILogger<GetBlogPostByIdHandler> _logger;

        public GetBlogPostByIdHandler(BlogDbContext context, IAuditService auditService, ILogger<GetBlogPostByIdHandler> logger)
        {
            _context = context;
            _auditService = auditService;
            _logger = logger;
        }

        public async Task<BlogPostResponseDto?> Handle(GetBlogPostByIdQuery request, CancellationToken cancellationToken)
        {
            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                var post = await _context.BlogPosts
                    .Include(p => p.Comments)
                    .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

                if (post == null)
                {
                    _logger.LogWarning("BlogPost with ID {PostId} not found", request.Id);
                    await transaction.RollbackAsync(cancellationToken);
                    return null;
                }

                var response = new BlogPostResponseDto
                {
                    Id = post.Id,
                    Title = post.Title,
                    Content = post.Content,
                    CreatedAt = post.CreatedAt,
                    Comments = post.Comments
                };

                await _auditService.LogAsync("Fetched BlogPost by Id", request.Id);
                await transaction.CommitAsync(cancellationToken);

                _logger.LogInformation("Fetched BlogPost with ID {PostId}", post.Id);
                return response;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.LogError(ex, "Error fetching BlogPost with ID {PostId}", request.Id);
                throw;
            }
        }
    }
}
