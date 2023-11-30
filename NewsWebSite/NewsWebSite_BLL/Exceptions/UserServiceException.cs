namespace NewsWebSite_BLL.Exceptions
{
    public class UserServiceException : Exception
    {
        public UserServiceException()
        {
        }

        public UserServiceException(string message)
            : base(message)
        {
        }

        public UserServiceException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
