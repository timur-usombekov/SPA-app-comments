using SPA_app_comments.Core.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace SPA_app_comments.Core.Domain.Entities
{
    public class Comment: Identity
    {
        public Guid UserId { get; set; }
        public Guid? ParentCommentId { get; set; }
        [StringLength(1000)]
        [Required]
        public string Text { get; set; } = null!;
        [StringLength(100)]
        public string? Url { get; set; }
        public byte[]? File { get; set; } 
        public DateTime CreatedAt { get; set; }


        public User User { get; set; }
        public Comment? ParentComment { get; set; }
        public ICollection<Comment> Replies { get; set; }
    }
}
