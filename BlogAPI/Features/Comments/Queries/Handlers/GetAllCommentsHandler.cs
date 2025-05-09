﻿using BlogAPI.Data;
using BlogAPI.Dtos.V1;
using BlogAPI.Models;
using BlogAPI.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Features.Comments
{
    public class GetAllCommentsHandler : IRequestHandler<GetAllCommentsQuery, List<CommentResponseDto>>
    {
        private readonly BlogDbContext _context;
        private readonly IAuditService _auditService;
        private readonly ILogger<GetAllCommentsHandler> _logger;

        public GetAllCommentsHandler(BlogDbContext context, IAuditService auditService, ILogger<GetAllCommentsHandler> logger)
        {
            _context = context;
            _auditService = auditService;
            _logger = logger;
        }

        public async Task<List<CommentResponseDto>> Handle(GetAllCommentsQuery request, CancellationToken cancellationToken)
        {
            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                var comments = await _context.Comments
                    .Select(c => new CommentResponseDto
                    {
                        Id = c.Id,
                        AuthorName = c.AuthorName,
                        Text = c.Text,
                        CreatedAt = c.CreatedAt,
                        PostId = c.PostId
                    })
                    .ToListAsync(cancellationToken);

                await _auditService.LogAsync("Fetched All Comments", 0);
                await transaction.CommitAsync(cancellationToken);

                _logger.LogInformation("Fetched {Count} comments", comments.Count);
                return comments;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.LogError(ex, "Error fetching all comments");
                throw;
            }
        }
    }
}
