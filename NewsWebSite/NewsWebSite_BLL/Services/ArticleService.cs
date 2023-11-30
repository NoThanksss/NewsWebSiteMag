using AutoMapper;
using Microsoft.Extensions.Logging;
using NewsWebSite_BLL.Exceptions;
using NewsWebSite_BLL.Interfaces;
using NewsWebSite_BLL.Models;
using NewsWebSite_DAL.Interfaces;
using NewsWebSite_DAL.Models;
using NewsWebSite_DAL.Repositories;

namespace NewsWebSite_BLL.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IMapper _mapper;
        private readonly IArticleRepository _articleRepository;
        private readonly ILogger<ArticleService> _logger;
        private readonly IAccountRepository _accountRepository;
        private readonly IMailService _mailService;

        public ArticleService(IMapper mapper, IArticleRepository articleRepository, IAccountRepository accountRepository, IMailService mailService,
            ILogger<ArticleService> logger)
        {
            _mapper = mapper;
            _articleRepository = articleRepository;
            _accountRepository = accountRepository;
            _mailService = mailService;
            _logger = logger;
        }

        public IEnumerable<Article> GetAllArticles() 
        {
            try 
            { 
                return _mapper.Map<List<Article>>(_articleRepository.GetAll());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GetAllArticles method");
                throw new ArticleServiceException(ex.Message);
            }
        }

        public Article AddNewArticle(Article article)
        {
            try 
            { 
                var articleToAdd = _mapper.Map<ArticleDB>(article);
                var newArticle = _articleRepository.AddEntity(articleToAdd);

                var author =  _accountRepository.GetById(newArticle.AccountId);
                foreach (var account in author.Subscibers)
                {
                     _mailService.SendNotification(account.Email, author.UserName, newArticle.Title);
                }

                return _mapper.Map<Article>(newArticle);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in AddNewArticle method");
                throw new ArticleServiceException(ex.Message);
            }
        }

        public void DeleteArticle(Guid id)
        {
            try 
            {
                _articleRepository.DeleteEntity(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in DeleteArticle method");
                throw new ArticleServiceException(ex.Message);
            }
        }

        public Article UpdateArticle(Article updatedArticle)
        {
            try 
            { 
                var mappedArticle = _mapper.Map<ArticleDB>(updatedArticle);

                return _mapper.Map<Article>(_articleRepository.UpdateEntity(mappedArticle));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in UpdateArticle method");
                throw new ArticleServiceException(ex.Message);
            }
        }

        public Article GetArticleById(Guid id) 
        {
            try 
            { 
                var article = _articleRepository.GetById(id);
                if (article == null)
                {
                    _logger.LogError($"Article with id {id} doesn't exist.");
                    throw new ArticleServiceException($"Article with id {id} doesn't exist.");
                }

                return _mapper.Map<Article>(article);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GetArticleById method");
                throw new ArticleServiceException(ex.Message);
            }
        }

        public IEnumerable<Article> GetArticlesByAccountId(Guid accountId)
        {
            try
            {
                return _mapper.Map<List<Article>>(_articleRepository.GetAll().Where(x => x.AccountId == accountId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GetAllArticles method");
                throw new ArticleServiceException(ex.Message);
            }
        }

        public IEnumerable<Article> GetArticlesByThemeId(Guid themeId)
        {
            try
            {
                return _mapper.Map<List<Article>>(_articleRepository.GetAllWithThemeDbs()
                    .Where(x => x.ArticleThemeDBs.Select(x => x.Id).Contains(themeId)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GetArticlesByThemeId method");
                throw new ArticleServiceException(ex.Message);
            }
        }

        public Article ChangeArticleThemes(Guid id, Guid[] themeIds)
        {
            try
            {
                var article = _articleRepository.GetById(id);
                if (article == null)
                {
                    _logger.LogError($"Article with id {id} doesn't exist.");
                    throw new ArticleServiceException($"Article with id {id} doesn't exist.");
                }

                var themesToDelete = article.ArticleThemeDBs.Select(x => x.Id).Except(themeIds).ToList();
                var themesToAdd = themeIds.Except(article.ArticleThemeDBs.Select(x => x.Id)).ToList();

                return _mapper.Map<Article>(_articleRepository.UpdateThemes(article, themesToAdd, themesToDelete));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in ChangeArticleThemes method");
                throw new ArticleServiceException(ex.Message);
            }
        }
    }
}
