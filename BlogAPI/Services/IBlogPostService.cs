using BlogAPI.Models;
using Microsoft.Extensions.Hosting;

namespace BlogAPI.Services
{
    public interface IBlogPostService
    {
        Task<bool> IDExistsAsync(int Id);
    }
}
