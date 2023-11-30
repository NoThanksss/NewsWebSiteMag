using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebSite_DAL.Models
{
    public class ArticleThemeDB
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual List<ArticleDB> ArticleDBs { get; set; }
    }
}
