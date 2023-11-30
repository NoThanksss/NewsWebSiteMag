namespace NewsWebSite_BLL.Exceptions
{
    public class ArticleThemeServiceException : Exception 
    {
        public ArticleThemeServiceException()
        {
        }

        public ArticleThemeServiceException(string message)
            : base(message)
        {
        }

        public ArticleThemeServiceException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
