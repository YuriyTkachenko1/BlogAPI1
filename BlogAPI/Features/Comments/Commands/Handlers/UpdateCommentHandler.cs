using BlogAPI.Data;
using BlogAPI.Services;
using MediatR;

namespace BlogAPI.Features.Comments
{
    public class UpdateCommentHandler : IRequestHandler<UpdateCommentCommand, bool>
    {
        private readonly BlogDbContext _context;
        private readonly IAuditService _auditService;
        private readonly ILogger<UpdateCommentHandler> _logger;

        public UpdateCommentHandler(BlogDbContext context, IAuditService auditService, ILogger<UpdateCommentHandler> logger)
        {
            _context = context;
            _auditService = auditService;
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
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

                comment.AuthorName = request.Dto.AuthorName;
                comment.Text = request.Dto.Text;
                comment.PostId = request.Dto.PostId;

                await _context.SaveChangesAsync(cancellationToken);
                await _auditService.LogAsync("Comment Updated", request.Id);
                await transaction.CommitAsync(cancellationToken);

                _logger.LogInformation("Updated Comment with ID {CommentId}", request.Id);
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.LogError(ex, "Error updating comment");
                throw;
            }
        }
    }
}
