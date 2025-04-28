using BlogAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Dtos.V1
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
        [MaxLength(50)]
        public ICollection<CommentResponseDto> Comments { get; set; } = new List<CommentResponseDto>();
    }
}
