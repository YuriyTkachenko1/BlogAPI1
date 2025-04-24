using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Dtos
{
    public class CommentDto
    {
        [Required]
        [StringLength(50)]
        public string AuthorName { get; set; } = string.Empty;

        [Required]
        [MinLength(1)]
        public string Text { get; set; } = string.Empty;

        [Required]
        public int PostId { get; set; }
    }
}
