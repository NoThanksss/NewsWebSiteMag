using NewsWebSite_BLL.Models;

namespace NewsWebSite_BLL.Interfaces
{
    public interface IArticleService
    {
        Task<IEnumerable<Article>> GetAllArticlesAsync();
        Task<Article> AddNewArticleAsync(Article article);
        Task DeleteArticleAsync(Guid id);
        Task<Article> UpdateArticleAsync(Article updatedAccount);
        Task<Article> GetArticleByIdAsync(Guid id);
        Task<IEnumerable<Article>> GetArticlesByAccountIdAsync(Guid accountId);
        Task<IEnumerable<Article>> GetArticlesByThemeIdAsync(Guid themeId);
        Task<Article> ChangeArticleThemesAsync(Guid id, Guid[] themeIds);
    }
}
