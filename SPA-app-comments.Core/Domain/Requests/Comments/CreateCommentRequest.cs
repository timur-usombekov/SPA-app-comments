using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SPA_app_comments.Core.Domain.Requests.Comments
{
    public class CreateCommentRequest
    {
        [Required(ErrorMessage = "UserName is required")]
        [StringLength(maximumLength: 100, MinimumLength = 1, ErrorMessage = "UserName can not be less than 1 or be above than 100 letters")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Text is required")]
        [StringLength(1000, ErrorMessage = "Text can't be above 1000 characters")]
        public string Text { get; set; }
        [Url(ErrorMessage = "Invalid url format")]
        public string? Url { get; set; }
        public IFormFile? File { get; set; }
        public Guid? ParentCommentId { get; set; }
    }

}
