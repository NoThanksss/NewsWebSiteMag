namespace NewsWebSite_BLL.Exceptions
{
    public class MailServiceException : Exception
    {
        public MailServiceException()
        {
        }

        public MailServiceException(string message)
            : base(message)
        {
        }

        public MailServiceException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
