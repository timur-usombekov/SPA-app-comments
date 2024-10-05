using System.ComponentModel.DataAnnotations;

namespace SPA_app_comments.Core.Domain.Entities.Base
{
    public abstract class Identity
    {
        [Key]
        public Guid Id { get; set; }
    }
}
