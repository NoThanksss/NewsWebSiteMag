using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebSite_DAL.Models
{
    public class CommentDB
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public Guid ArticleId { get; set; }
        public string Text { get; set; }
    }
}
