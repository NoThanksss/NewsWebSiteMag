using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebSite_DAL.Models
{
    public class ArticleDB
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Text { get; set; }
        public Guid AccountId { get; set; }
        public virtual List<CommentDB> CommentDBs { get; set; }
        public virtual List<ArticleThemeDB> ArticleThemeDBs { get; set; }
    }
}
