using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Dtos
{
    public class CommentDto
    {
        [Required]
        [StringLength(50)]
        public string Author { get; set; } = string.Empty;

        [Required]
        [MinLength(1)]
        public string Message { get; set; } = string.Empty;

        [Required]
        public int BlogPostId { get; set; }
    }
}
