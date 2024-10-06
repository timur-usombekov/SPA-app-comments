using SPA_app_comments.Core.Domain.Entities;

namespace SPA_app_comments.Core.Dto
{
    public class UserResponse
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
