using System.ComponentModel.DataAnnotations;
using BlogAPI.Models;

namespace BlogAPI.Dtos.V1
{
    public class BlogPostDto
    {
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MinLength(10)]
        public string Content { get; set; } = string.Empty;
    }
}
