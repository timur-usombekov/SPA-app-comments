using Microsoft.AspNetCore.Http;

namespace SPA_app_comments.Core.Domain.Requests.Comments
{
    public class CreateCommentRequest
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Text { get; set; }
        public string? Url { get; set; }
        public IFormFile? File { get; set; }
        public Guid? ParentCommentId { get; set; }
    }

}
