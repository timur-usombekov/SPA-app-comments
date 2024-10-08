namespace SPA_app_comments.Core.Helpers.Exeptions
{
    [Serializable]
    public class FileSizeException : Exception
    {
        public FileSizeException() { }
        public FileSizeException(string message) : base(message) { }
        public FileSizeException(string message, Exception inner) : base(message, inner) { }
        protected FileSizeException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
