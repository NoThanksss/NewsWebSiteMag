using NewsWebSite_BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebSite_BLL.Interfaces
{
    public interface IArticleService
    {
        IEnumerable<Article> GetAllArticles();
        Article AddNewArticle(Article article);
        void DeleteArticle(Guid id);
        Article UpdateArticle(Article updatedAccount);
        Article GetArticleById(Guid id);
        IEnumerable<Article> GetArticlesByAccountId(Guid accountId);
        IEnumerable<Article> GetArticlesByThemeId(Guid themeId);
        Article ChangeArticleThemes(Guid id, Guid[] themeIds);
    }
}
