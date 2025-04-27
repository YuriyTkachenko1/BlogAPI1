using BlogAPI.Dtos.V1;
using MediatR;

namespace BlogAPI.Features.Comments
{
    public record GetCommentByIdQuery(int Id) : IRequest<CommentResponseDto?>;
}
