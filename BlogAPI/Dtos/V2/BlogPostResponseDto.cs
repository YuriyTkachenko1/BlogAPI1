using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Dtos.V2
{
    public class BlogPostResponseDto
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
