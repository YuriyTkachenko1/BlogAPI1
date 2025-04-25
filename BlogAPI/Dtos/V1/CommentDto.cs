using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Dtos.V1
{
    public class CommentDto
    {
        [Required]
        [StringLength(50)]
        public string AuthorName { get; set; } = string.Empty;

        [Required]
        [StringLength(1000, MinimumLength = 1)]
        public string Text { get; set; } = string.Empty;

        [Required]
        public int PostId { get; set; }
    }
}
