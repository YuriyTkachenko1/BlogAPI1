using BlogAPI.Dtos.V1;
using MediatR;

namespace BlogAPI.Features.Comments
{
    public record GetAllCommentsQuery() : IRequest<List<CommentResponseDto>>;
}
