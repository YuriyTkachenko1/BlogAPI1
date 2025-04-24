using BlogAPI.Data;
using BlogAPI.Features.Posts.Commands.V1;
using BlogAPI.Services;
using MediatR;

namespace BlogAPI.Features.Posts.Commands.Handlers.V1
{
    public class UpdateBlogPostHandler : IRequestHandler<UpdateBlogPostCommand, bool>
    {
        private readonly BlogDbContext _context;
        private readonly IAuditService _auditService;
        private readonly ILogger<UpdateBlogPostHandler> _logger;

        public UpdateBlogPostHandler(BlogDbContext context, IAuditService auditService, ILogger<UpdateBlogPostHandler> logger)
        {
            _context = context;
            _auditService = auditService;
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateBlogPostCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var post = await _context.BlogPosts.FindAsync(new object[] { request.Id }, cancellationToken);
                if (post == null)
                {
                    _logger.LogWarning("BlogPost with ID {PostId} not found for update", request.Id);
                    await transaction.RollbackAsync(cancellationToken);
                    return false;
                }

                post.Title = request.Dto.Title;
                post.Content = request.Dto.Content;

                _context.BlogPosts.Update(post);
                await _context.SaveChangesAsync(cancellationToken);
                await _auditService.LogAsync("BlogPost Updated", post.Id);

                await transaction.CommitAsync(cancellationToken);
                _logger.LogInformation("BlogPost updated with ID {PostId}", post.Id);
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.LogError(ex, "Error updating blog post with ID {PostId}", request.Id);
                throw;
            }
        }
    }
}
