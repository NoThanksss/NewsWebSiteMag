using NewsWebSite_BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebSite_BLL.Interfaces
{
    public interface IArticleThemeService
    {
        IEnumerable<ArticleTheme> GetAllArticleThemes();
        ArticleTheme AddNewArticleTheme(ArticleTheme account);
        void DeleteArticleTheme(Guid id);
        ArticleTheme UpdateArticleTheme(ArticleTheme updatedAccount);
        ArticleTheme GetArticleThemeById(Guid id);
        IEnumerable<ArticleTheme> GetArticleThemesByArticleId(Guid articleId);
    }
}
