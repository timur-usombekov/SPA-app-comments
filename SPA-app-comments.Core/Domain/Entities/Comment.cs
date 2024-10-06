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
        public byte[]? File { get; set; }
        public byte[]? Photo { get; set; }
        public DateTime CreatedAt { get; } = DateTime.Now;


        public User User { get; set; }
        public Comment? ParentComment { get; set; }
        public ICollection<Comment> Replies { get; set; }
    }
}
