using MediatR;
using BlogAPI.Models;
using Microsoft.EntityFrameworkCore;
using BlogAPI.Dtos.V1;

namespace BlogAPI.Features.Posts.Commands.V1
{
    public record CreateBlogPostCommand(BlogPostDto Dto) : IRequest<int>;
}
