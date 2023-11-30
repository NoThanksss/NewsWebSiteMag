using NewsWebSite_BLL.Attributes;

namespace NewsWebSite_BLL.Models
{
    public class ArticleTheme
    {
        [SwaggerExclude]
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
