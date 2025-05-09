﻿using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models
{
    public class BlogPost
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [MaxLength(50)]
        public string CreatedBy { get; set; } = string.Empty;

        [MaxLength(50)]
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
