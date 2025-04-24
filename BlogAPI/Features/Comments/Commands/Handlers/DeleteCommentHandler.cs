using BlogAPI.Data;
using BlogAPI.Services;
using MediatR;

namespace BlogAPI.Features.Comments
{
    public class DeleteCommentHandler : IRequestHandler<DeleteCommentCommand, bool>
    {
        private readonly BlogDbContext _context;
        private readonly IAuditService _auditService;
        private readonly ILogger<DeleteCommentHandler> _logger;

        public DeleteCommentHandler(BlogDbContext context, IAuditService auditService, ILogger<DeleteCommentHandler> logger)
        {
            _context = context;
            _auditService = auditService;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var comment = await _context.Comments.FindAsync(new object[] { request.Id }, cancellationToken);
                if (comment == null)
                {
                    _logger.LogWarning("Comment with ID {CommentId} not found", request.Id);
                    await transaction.RollbackAsync(cancellationToken);
                    return false;
                }

                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync(cancellationToken);
                await _auditService.LogAsync("Comment Deleted", request.Id);
                await transaction.CommitAsync(cancellationToken);

                _logger.LogInformation("Deleted Comment with ID {CommentId}", request.Id);
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.LogError(ex, "Error deleting comment");
                throw;
            }
        }
    }
}
