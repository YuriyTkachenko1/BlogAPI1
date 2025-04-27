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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateCommentHandler(BlogDbContext context, IAuditService auditService, ILogger<CreateCommentHandler> logger, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _auditService = auditService;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<int> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var username = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "UnknownUser";
            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var comment = new Comment
                {
                    AuthorName = request.Dto.AuthorName,
                    Text = request.Dto.Text,
                    PostId = request.Dto.PostId,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = username
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
