using SPA_app_comments.Core.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace SPA_app_comments.Core.Domain.Entities
{
    public class Comment: Identity
    {
        public Guid UserId { get; set; }
        public Guid? ParentCommentId { get; set; }
        [StringLength(1000)]
        public string? Text { get; set; }
        [Required]
        public byte[]? File { get; set; }
        public byte[]? Photo { get; set; }
        public DateTime CreatedAt { get; } = DateTime.Now;


        public User User { get; set; } = null!;
        public Comment? ParentComment { get; set; }
        public ICollection<Comment> Replies { get; set; }
    }
}
