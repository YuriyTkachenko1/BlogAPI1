﻿using BlogAPI.Data;
using BlogAPI.Features.Posts;
using BlogAPI.Services;
using MediatR;

namespace BlogAPI.Features.Posts
{
    public class DeleteBlogPostHandler : IRequestHandler<DeleteBlogPostCommand, bool>
    {
        private readonly BlogDbContext _context;
        private readonly IAuditService _auditService;
        private readonly ILogger<DeleteBlogPostHandler> _logger;
        private readonly IBlogPostService _blogPostService;

        public DeleteBlogPostHandler(BlogDbContext context, IAuditService auditService, ILogger<DeleteBlogPostHandler> logger, IBlogPostService blogPostService)
        {
            _context = context;
            _auditService = auditService;
            _logger = logger;
            _blogPostService = blogPostService;
        }

        public async Task<bool> Handle(DeleteBlogPostCommand request, CancellationToken cancellationToken)
        {
            await _auditService.LogAsync("BlogPost Deleted", request.Id);
            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var postExists = await _blogPostService.IDExistsAsync(request.Id);
                if (!postExists)
                {
                    _logger.LogWarning("BlogPost with ID {PostId} does not exist", request.Id);
                    await transaction.RollbackAsync(cancellationToken);
                    return false;
                }
                var post = await _context.BlogPosts.FindAsync(new object[] { request.Id }, cancellationToken);
                if (post == null)
                {
                    _logger.LogWarning("Post with ID {PostId} not found", request.Id);
                    await transaction.RollbackAsync(cancellationToken);
                    return false;
                }
                _context.BlogPosts.Remove(post);
                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                _logger.LogInformation("Deleted BlogPost with ID {PostId}", request.Id);
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.LogError(ex, "Error deleting BlogPost with ID {PostId}", request.Id);
                throw;
            }
        }
    }
}
