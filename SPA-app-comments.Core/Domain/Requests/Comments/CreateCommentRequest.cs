using Microsoft.AspNetCore.Http;

namespace SPA_app_comments.Core.Domain.Requests.Comments
{
    public record CreateCommentRequest(string UserName, string Email, string? Url, IFormFile? File, string Text, Guid? ParentCommentId = null);

}
