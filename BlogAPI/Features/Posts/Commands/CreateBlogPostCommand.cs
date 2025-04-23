using MediatR;
using BlogAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Features.Posts
{
    public record CreateBlogPostCommand(string Title, string Content) : IRequest<int>;
}
