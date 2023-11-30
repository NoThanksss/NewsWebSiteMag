using NewsWebSite_BLL.Attributes;

namespace NewsWebSite_BLL.Models
{
    public class Article
    {
        [SwaggerExclude]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Text { get; set; }
        public Guid AccountId { get; set; }
    }
}
