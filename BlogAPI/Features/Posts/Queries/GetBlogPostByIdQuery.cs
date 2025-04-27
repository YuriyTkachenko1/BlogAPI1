using BlogAPI.Dtos.V1;
using MediatR;
using Microsoft.Extensions.Hosting;

namespace BlogAPI.Features.Posts
{
    public record GetBlogPostByIdQuery(int Id) : IRequest<BlogPostResponseDto?>;

}
