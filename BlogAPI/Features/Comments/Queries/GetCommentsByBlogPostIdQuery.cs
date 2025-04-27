using BlogAPI.Dtos.V1;
using MediatR;

namespace BlogAPI.Features.Comments
{
    public record GetCommentsByBlogPostIdQuery(int BlogPostId) : IRequest<List<CommentResponseDto>>;
}
