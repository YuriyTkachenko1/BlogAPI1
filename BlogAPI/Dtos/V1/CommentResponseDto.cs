using BlogAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Dtos.V1
{
    public class CommentResponseDto
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string AuthorName { get; set; } = string.Empty;
        [Required]
        [MaxLength(1000)]
        public string Text { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [MaxLength(50)]

        public int PostId { get; set; }
        //public BlogPost BlogPost { get; set; } = null!;
    }
}
