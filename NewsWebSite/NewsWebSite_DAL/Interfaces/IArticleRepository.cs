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
        IQueryable<ArticleDB> GetAllWithThemeDbsAsync();
        Task<ArticleDB> UpdateThemesAsync(ArticleDB entity, List<Guid> themesToAdd, List<Guid> themesToRemove);
    }
}
