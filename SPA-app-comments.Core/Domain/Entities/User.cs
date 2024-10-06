using SPA_app_comments.Core.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace SPA_app_comments.Core.Domain.Entities
{
    public class User: Identity
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        public ICollection<Comment> Comments { get; set; }
    }
}
