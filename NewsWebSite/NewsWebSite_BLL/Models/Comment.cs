using NewsWebSite_BLL.Attributes;

namespace NewsWebSite_BLL.Models
{
    public class Comment
    {
        [SwaggerExclude]
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public Guid ArticleId { get; set; }
        public string Text { get; set; }
    }
}
