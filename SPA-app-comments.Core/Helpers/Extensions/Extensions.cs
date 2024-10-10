using SPA_app_comments.Core.Domain.Entities;
using SPA_app_comments.Core.Dto;

namespace SPA_app_comments.Core.Helpers.Extensions
{
    public static class Extensions
    {
        public static CommentResponse ToCommentResponse(this Comment comment)
        {
            var response = new CommentResponse()
            {
                Id = comment.Id,
                CreatedAt = comment.CreatedAt,
                File = comment.File,
                ParentCommentId = comment.ParentCommentId,
                Text = comment.Text,
                Url = comment.Url,
                User = comment.User.ToUserResponse(),
            };

            return response;
        }

        public static UserResponse ToUserResponse(this User user)
        {
            var response = new UserResponse()
            {
                Name = user.Name,
                Email = user.Email,
            };

            return response;
        }
    }
}
