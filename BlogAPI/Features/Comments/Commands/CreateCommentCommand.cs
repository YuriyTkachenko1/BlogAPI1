﻿using BlogAPI.Dtos.V1;
using MediatR;

namespace BlogAPI.Features.Comments
{
    public record CreateCommentCommand(CommentDto Dto) : IRequest<int>;
}
