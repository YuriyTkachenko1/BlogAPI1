using BlogAPI.Data;
using BlogAPI.Dtos.V1;
using BlogAPI.Models;
using BlogAPI.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Features.Comments
{
    public class GetCommentByIdHandler : IRequestHandler<GetCommentByIdQuery, CommentResponseDto?>
    {
        private readonly BlogDbContext _context;
        private readonly IAuditService _auditService;
        private readonly ILogger<GetCommentByIdHandler> _logger;

        public GetCommentByIdHandler(BlogDbContext context, IAuditService auditService, ILogger<GetCommentByIdHandler> logger)
        {
            _context = context;
            _auditService = auditService;
            _logger = logger;
        }

        public async Task<CommentResponseDto?> Handle(GetCommentByIdQuery request, CancellationToken cancellationToken)
        {
            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                var comment = await _context.Comments
                    .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

                if (comment == null)
                {
                    _logger.LogWarning("Comment with ID {CommentId} not found", request.Id);
                    await transaction.RollbackAsync(cancellationToken);
                    return null;
                }

                var response = new CommentResponseDto
                {
                    Id = comment.Id,
                    AuthorName = comment.AuthorName,
                    Text = comment.Text,
                    CreatedAt = comment.CreatedAt,
                    PostId = comment.PostId
                };

                await _auditService.LogAsync("Fetched Comment by Id", request.Id);
                await transaction.CommitAsync(cancellationToken);

                _logger.LogInformation("Fetched Comment with ID {CommentId}", comment.Id);
                return response;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.LogError(ex, "Error fetching Comment with ID {CommentId}", request.Id);
                throw;
            }
        }
    }
}
