using BlogAPI.Data;
using BlogAPI.Models;
using MediatR;

namespace BlogAPI.Features.Comments
{
    public class GetCommentByIdHandler : IRequestHandler<GetCommentByIdQuery, Comment?>
    {
        private readonly BlogDbContext _context;

        public GetCommentByIdHandler(BlogDbContext context)
        {
            _context = context;
        }

        public async Task<Comment?> Handle(GetCommentByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Comments.FindAsync(new object[] { request.Id }, cancellationToken);
        }
    }
}
