namespace NewsWebSite_BLL.Exceptions
{
    public class AccountServiceException : Exception 
    {
        public AccountServiceException()
        {
        }

        public AccountServiceException(string message)
            : base(message)
        {
        }

        public AccountServiceException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
