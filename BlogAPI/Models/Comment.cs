using Microsoft.Extensions.Hosting;

namespace BlogAPI.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int PostId { get; set; }
        public BlogPost BlogPost { get; set; } = null!;
    }
}
