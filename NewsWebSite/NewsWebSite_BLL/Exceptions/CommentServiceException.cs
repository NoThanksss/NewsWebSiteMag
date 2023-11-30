namespace NewsWebSite_BLL.Exceptions
{
    public class CommentServiceException : Exception
    {
        public CommentServiceException()
        {
        }

        public CommentServiceException(string message)
            : base(message)
        {
        }

        public CommentServiceException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
