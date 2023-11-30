using NewsWebSite_BLL.Models;

namespace NewsWebSite_BLL.Interfaces
{
    public interface IArticleThemeService
    {
        Task<IEnumerable<ArticleTheme>> GetAllArticleThemesAsync();
        Task<ArticleTheme> AddNewArticleThemeAsync(ArticleTheme account);
        Task DeleteArticleThemeAsync(Guid id);
        Task<ArticleTheme> UpdateArticleThemeAsync(ArticleTheme updatedAccount);
        Task<ArticleTheme> GetArticleThemeByIdAsync(Guid id);
        Task<IEnumerable<ArticleTheme>> GetArticleThemesByArticleIdAsync(Guid articleId);
    }
}
