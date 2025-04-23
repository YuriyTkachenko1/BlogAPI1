using BlogAPI.Models;
using Microsoft.Extensions.Hosting;

namespace BlogAPI.Services
{
    public interface IBlogService
    {
        Task<int> CreatePostAsync(string title, string content);
        Task<BlogPost?> GetPostAsync(int id);
    }
}
