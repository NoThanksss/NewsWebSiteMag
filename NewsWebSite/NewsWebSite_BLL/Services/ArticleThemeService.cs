using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using NewsWebSite_BLL.Exceptions;
using NewsWebSite_BLL.Interfaces;
using NewsWebSite_BLL.Models;
using NewsWebSite_DAL.Interfaces;
using NewsWebSite_DAL.Models;

namespace NewsWebSite_BLL.Services
{
    public class ArticleThemeService : IArticleThemeService
    {
        private readonly IMapper _mapper;
        private readonly IArticleThemeRepository _articleThemeRepository;
        private readonly IArticleRepository _articleRepository;
        private readonly ILogger<ArticleThemeService> _logger;
        private readonly IMemoryCache _cache;

        public ArticleThemeService(IMapper mapper, IArticleThemeRepository articleThemeRepository,
            ILogger<ArticleThemeService> logger, IArticleRepository articleRepository, IMemoryCache cache)
        {
            _mapper = mapper;
            _articleThemeRepository = articleThemeRepository;
            _logger = logger;
            _articleRepository = articleRepository;
            _cache = cache;
        }

        public async Task<IEnumerable<ArticleTheme>> GetAllArticleThemesAsync() 
        {
            try 
            { 
                return _mapper.Map<List<ArticleTheme>>(await _articleThemeRepository.GetAllAsync().ToListAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GetAllArticleThemes method");
                throw new ArticleThemeServiceException(ex.Message);
            }
        }

        public async Task <ArticleTheme> AddNewArticleThemeAsync(ArticleTheme articleTheme)
        {
            try 
            { 
                var articleThemeToAdd = _mapper.Map<ArticleThemeDB>(articleTheme);
                var newArticleTheme = await _articleThemeRepository.AddEntityAsync(articleThemeToAdd);
                if (newArticleTheme != null)
                {
                    _cache.Set(newArticleTheme.Id, newArticleTheme,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }

                return _mapper.Map<ArticleTheme>(newArticleTheme);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in AddNewArticleTheme method");
                throw new ArticleThemeServiceException(ex.Message);
            }
        }

        public async Task DeleteArticleThemeAsync(Guid id)
        {
            try 
            { 
                await _articleThemeRepository.DeleteEntityAsync(id);
                _cache.Remove(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in DeleteArticleTheme method");
                throw new ArticleThemeServiceException(ex.Message);
            }
        }

        public async Task <ArticleTheme> UpdateArticleThemeAsync(ArticleTheme updatedarticleTheme)
        {
            try 
            { 
                var mappedArticleTheme = _mapper.Map<ArticleThemeDB>(updatedarticleTheme);
                var result = await _articleThemeRepository.UpdateEntityAsync(mappedArticleTheme);
                if (result != null)
                {
                    _cache.Set(result.Id, result,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }

                return _mapper.Map<ArticleTheme>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in UpdateArticleTheme method");
                throw new ArticleThemeServiceException(ex.Message);
            }
        }

        public async Task<ArticleTheme> GetArticleThemeByIdAsync(Guid id) 
        {
            try 
            {
                ArticleThemeDB articleTheme = null;
                if (!_cache.TryGetValue(id, out articleTheme))
                {
                    articleTheme = await _articleThemeRepository.GetByIdAsync(id);
                    if (articleTheme == null)
                    {
                        _logger.LogError($"ArticleTheme with id {id} doesn't exist.");
                        throw new ArticleThemeServiceException($"ArticleTheme with id {id} doesn't exist.");
                    }
                    _cache.Set(articleTheme.Id, articleTheme,
                        new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }

                return _mapper.Map<ArticleTheme>(articleTheme);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GetArticleThemeById method");
                throw new ArticleThemeServiceException(ex.Message);
            }
        }

        public async Task<IEnumerable<ArticleTheme>> GetArticleThemesByArticleIdAsync(Guid articleId)
        {
            try
            {
                var article = await _articleRepository.GetByIdAsync(articleId);
                return _mapper.Map<List<ArticleTheme>>(article.ArticleThemeDBs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GetAllArticleThemes method");
                throw new ArticleThemeServiceException(ex.Message);
            }
        }
    }
}
