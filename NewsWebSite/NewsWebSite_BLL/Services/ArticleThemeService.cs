

using AutoMapper;
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

        public ArticleThemeService(IMapper mapper, IArticleThemeRepository articleThemeRepository,
            ILogger<ArticleThemeService> logger, IArticleRepository articleRepository)
        {
            _mapper = mapper;
            _articleThemeRepository = articleThemeRepository;
            _logger = logger;
            _articleRepository = articleRepository;
        }

        public IEnumerable<ArticleTheme> GetAllArticleThemes() 
        {
            try 
            { 
                return _mapper.Map<List<ArticleTheme>>(_articleThemeRepository.GetAll());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GetAllArticleThemes method");
                throw new ArticleThemeServiceException(ex.Message);
            }
        }

        public ArticleTheme AddNewArticleTheme(ArticleTheme articleTheme)
        {
            try 
            { 
                var articleThemeToAdd = _mapper.Map<ArticleThemeDB>(articleTheme);
                var newarticleTheme = _articleThemeRepository.AddEntity(articleThemeToAdd);

                return _mapper.Map<ArticleTheme>(newarticleTheme);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in AddNewArticleTheme method");
                throw new ArticleThemeServiceException(ex.Message);
            }
        }

        public void DeleteArticleTheme(Guid id)
        {
            try 
            { 
                _articleThemeRepository.DeleteEntity(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in DeleteArticleTheme method");
                throw new ArticleThemeServiceException(ex.Message);
            }
        }

        public ArticleTheme UpdateArticleTheme(ArticleTheme updatedarticleTheme)
        {
            try 
            { 
                var mappedarticleTheme = _mapper.Map<ArticleThemeDB>(updatedarticleTheme);

                return _mapper.Map<ArticleTheme>(_articleThemeRepository.UpdateEntity(mappedarticleTheme));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in UpdateArticleTheme method");
                throw new ArticleThemeServiceException(ex.Message);
            }
        }

        public ArticleTheme GetArticleThemeById(Guid id) 
        {
            try 
            { 
                var articleTheme = _articleThemeRepository.GetById(id);
                if (articleTheme == null)
                {
                    _logger.LogError($"ArticleTheme with id {id} doesn't exist.");
                    throw new ArticleThemeServiceException($"ArticleTheme with id {id} doesn't exist.");
                }

                return _mapper.Map<ArticleTheme>(articleTheme);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GetArticleThemeById method");
                throw new ArticleThemeServiceException(ex.Message);
            }
        }

        public IEnumerable<ArticleTheme> GetArticleThemesByArticleId(Guid articleId)
        {
            try
            {
                return _mapper.Map<List<ArticleTheme>>(_articleRepository.GetById(articleId).ArticleThemeDBs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GetAllArticleThemes method");
                throw new ArticleThemeServiceException(ex.Message);
            }
        }
    }
}
