using BlogAPI.Data;
using BlogAPI.Models;
using BlogAPI.Services;
using MediatR;

namespace BlogAPI.Features.Comments
{
    public class CreateCommentHandler : IRequestHandler<CreateCommentCommand, int>
    {
        private readonly BlogDbContext _context;
        private readonly IAuditService _auditService;
        private readonly ILogger<CreateCommentHandler> _logger;

        public CreateCommentHandler(BlogDbContext context, IAuditService auditService, ILogger<CreateCommentHandler> logger)
        {
            _context = context;
            _auditService = auditService;
            _logger = logger;
        }

        public async Task<int> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var comment = new Comment
                {
                    AuthorName = request.Dto.AuthorName,
                    Text = request.Dto.Text,
                    PostId = request.Dto.PostId
                };

                await _context.Comments.AddAsync(comment, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                await _auditService.LogAsync("Comment Created", comment.Id);
                await transaction.CommitAsync(cancellationToken);

                _logger.LogInformation("Created Comment with ID {CommentId}", comment.Id);
                return comment.Id;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.LogError(ex, "Error creating comment");
                throw;
            }
        }
    }
}
