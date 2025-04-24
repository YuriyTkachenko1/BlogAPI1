using MediatR;
using BlogAPI.Models;
using Microsoft.EntityFrameworkCore;
using BlogAPI.Dtos;

namespace BlogAPI.Features.Posts
{
    public record CreateBlogPostCommand(BlogPostDto Dto) : IRequest<int>;
}
