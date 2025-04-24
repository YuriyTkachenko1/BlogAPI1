using BlogAPI.Dtos;
using BlogAPI.Models;
using Microsoft.Extensions.Hosting;

namespace BlogAPI.Services
{
    public interface IBlogService
    {
        Task<int> CreatePostAsync(BlogPostDto Dto);
        Task<BlogPost?> GetPostAsync(int id);
    }
}
