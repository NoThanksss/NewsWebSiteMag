using NewsWebSite_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebSite_DAL.Interfaces
{
    public interface IArticleRepository :IBaseRepository<ArticleDB>
    {
        IEnumerable<ArticleDB> GetAllWithThemeDbs();
        ArticleDB UpdateThemes(ArticleDB entity, List<Guid> themesToAdd, List<Guid> themesToRemove);
    }
}
