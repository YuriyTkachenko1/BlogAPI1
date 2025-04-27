using BlogAPI.Data;
using BlogAPI.Dtos.V1;
using BlogAPI.Models;
using BlogAPI.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BlogAPI.Features.Comments
{
    public class GetCommentsByBlogPostIdHandler : IRequestHandler<GetCommentsByBlogPostIdQuery, List<CommentResponseDto>>
    {
        private readonly BlogDbContext _context;
        private readonly IAuditService _auditService;
        private readonly ILogger<GetCommentsByBlogPostIdHandler> _logger;

        public GetCommentsByBlogPostIdHandler(BlogDbContext context, IAuditService auditService, ILogger<GetCommentsByBlogPostIdHandler> logger)
        {
            _context = context;
            _auditService = auditService;
            _logger = logger;
        }

        public async Task<List<CommentResponseDto>> Handle(GetCommentsByBlogPostIdQuery request, CancellationToken cancellationToken)
        {
            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                var comments = await _context.Comments
                    .Where(c => c.PostId == request.BlogPostId)
                    .Select(c => new CommentResponseDto
                    {
                        Id = c.Id,
                        AuthorName = c.AuthorName,
                        Text = c.Text,
                        CreatedAt = c.CreatedAt,
                        PostId = c.PostId
                    })
                    .ToListAsync(cancellationToken);

                await _auditService.LogAsync("Fetched Comments for BlogPost", request.BlogPostId);
                await transaction.CommitAsync(cancellationToken);

                _logger.LogInformation("Fetched {Count} comments for BlogPostId {BlogPostId}", comments.Count, request.BlogPostId);
                return comments;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.LogError(ex, "Error fetching comments for BlogPostId {BlogPostId}", request.BlogPostId);
                throw;
            }
        }
    }
}
