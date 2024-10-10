namespace SPA_app_comments.Core.Dto
{
    public class CommentResponse
    {
        public Guid Id { get; set; }
        public Guid? ParentCommentId { get; set; }
        public UserResponse User { get; set; }
        public string Text { get; set; } = null!;
        public string? Url { get; set; } = null!;
        public byte[]? File { get; set; }
        public string? FileExtension { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
