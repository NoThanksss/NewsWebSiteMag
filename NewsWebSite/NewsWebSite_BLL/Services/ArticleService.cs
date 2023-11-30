using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using NewsWebSite_BLL.Exceptions;
using NewsWebSite_BLL.Interfaces;
using NewsWebSite_BLL.Models;
using NewsWebSite_DAL.Interfaces;
using NewsWebSite_DAL.Models;
using System.Security.Principal;

namespace NewsWebSite_BLL.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IMapper _mapper;
        private readonly IArticleRepository _articleRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IMailService _mailService;
        private readonly ILogger<ArticleService> _logger;
        private readonly IMemoryCache _cache;

        public ArticleService(IMapper mapper, IArticleRepository articleRepository, IAccountRepository accountRepository, IMailService mailService,
            ILogger<ArticleService> logger, IMemoryCache cache)
        {
            _mapper = mapper;
            _articleRepository = articleRepository;
            _accountRepository = accountRepository;
            _mailService = mailService;
            _logger = logger;
            _cache = cache;
        }

        public async Task<IEnumerable<Article>> GetAllArticlesAsync() 
        {
            try 
            {
                var articles = await _articleRepository.GetAllAsync().ToListAsync();
                return _mapper.Map<List<Article>>(articles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GetAllArticles method");
                throw new ArticleServiceException(ex.Message);
            }
        }

        public async Task<Article> AddNewArticleAsync(Article article)
        {
            try 
            { 
                var articleToAdd = _mapper.Map<ArticleDB>(article);
                var newArticle = await _articleRepository.AddEntityAsync(articleToAdd);
                if (newArticle != null)
                {
                    _cache.Set(newArticle.Id, newArticle,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
                var author = await _accountRepository.GetByIdAsync(newArticle.AccountId);
                foreach (var account in author.Subscibers) {
                    await _mailService.SendNotification(account.Email, author.UserName, newArticle.Title);
                }

                return _mapper.Map<Article>(newArticle);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in AddNewArticle method");
                throw new ArticleServiceException(ex.Message);
            }
        }

        public async Task DeleteArticleAsync(Guid id)
        {
            try 
            {
                await _articleRepository.DeleteEntityAsync(id);
                _cache.Remove(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in DeleteArticle method");
                throw new ArticleServiceException(ex.Message);
            }
        }

        public async Task<Article> UpdateArticleAsync(Article updatedArticle)
        {
            try 
            { 
                var mappedArticle = _mapper.Map<ArticleDB>(updatedArticle);
                var result = await _articleRepository.UpdateEntityAsync(mappedArticle);
                if (result != null)
                {
                    _cache.Set(result.Id, result,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }

                return _mapper.Map<Article>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in UpdateArticle method");
                throw new ArticleServiceException(ex.Message);
            }
        }

        public async Task<Article> GetArticleByIdAsync(Guid id) 
        {
            try 
            {
                ArticleDB article = null;
                if (!_cache.TryGetValue(id, out article))
                {
                    article = await _articleRepository.GetByIdAsync(id);
                    if (article == null)
                    {
                        _logger.LogError($"Article with id {id} doesn't exist.");
                        throw new ArticleServiceException($"Article with id {id} doesn't exist.");
                    }
                    _cache.Set(article.Id, article,
                        new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
                return _mapper.Map<Article>(article);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GetArticleById method");
                throw new ArticleServiceException(ex.Message);
            }
        }

        public async Task<IEnumerable<Article>> GetArticlesByAccountIdAsync(Guid accountId)
        {
            try
            {
                var articles = await _articleRepository.GetAllAsync().Where(x => x.AccountId == accountId).ToListAsync();
                return _mapper.Map<List<Article>>(articles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GetAllArticles method");
                throw new ArticleServiceException(ex.Message);
            }
        }

        public async Task<IEnumerable<Article>> GetArticlesByThemeIdAsync(Guid themeId)
        {
            try
            {
                var articles = await _articleRepository.GetAllWithThemeDbsAsync()
                    .Where(x => x.ArticleThemeDBs.Select(x => x.Id).Contains(themeId)).ToListAsync();
                return _mapper.Map<List<Article>>(articles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GetArticlesByThemeId method");
                throw new ArticleServiceException(ex.Message);
            }
        }

        public async Task<Article> ChangeArticleThemesAsync(Guid id, Guid[] themeIds)
        {
            try
            {
                var article = await _articleRepository.GetByIdAsync(id);
                if (article == null)
                {
                    _logger.LogError($"Article with id {id} doesn't exist.");
                    throw new ArticleServiceException($"Article with id {id} doesn't exist.");
                }

                var themesToDelete = article.ArticleThemeDBs.Select(x => x.Id).Except(themeIds).ToList();
                var themesToAdd = themeIds.Except(article.ArticleThemeDBs.Select(x => x.Id)).ToList();

                var result = await _articleRepository.UpdateThemesAsync(article, themesToAdd, themesToDelete);

                return _mapper.Map<Article>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in ChangeArticleThemes method");
                throw new ArticleServiceException(ex.Message);
            }
        }
    }
}
