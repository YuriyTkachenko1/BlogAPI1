using BlogAPI.Data;
using BlogAPI.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Features.Comments
{
    public class GetAllCommentsHandler : IRequestHandler<GetAllCommentsQuery, List<Comment>>
    {
        private readonly BlogDbContext _context;

        public GetAllCommentsHandler(BlogDbContext context)
        {
            _context = context;
        }

        public async Task<List<Comment>> Handle(GetAllCommentsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Comments.ToListAsync(cancellationToken);
        }
    }
}
