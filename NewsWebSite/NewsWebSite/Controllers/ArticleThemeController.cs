using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsWebSite_BLL.Exceptions;
using NewsWebSite_BLL.Interfaces;
using NewsWebSite_BLL.Models;

namespace NewsWebSite.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ArticleThemeController : Controller
    {
        private IArticleThemeService _articleThemeService;
        private readonly ILogger<ArticleThemeController> _logger;
        public ArticleThemeController(IArticleThemeService articleThemeService, ILogger<ArticleThemeController> logger)
        {
            _articleThemeService = articleThemeService;
            _logger = logger;

        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddNewArticleTheme([FromBody] ArticleTheme articleTheme)
        {
            try
            {
                var result = _articleThemeService.AddNewArticleTheme(articleTheme);

                return Ok(result);
            }
            catch (ArticleThemeServiceException ex)
            {
                _logger.LogError(ex, "Exception in AddNewArticleTheme method");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Exception in AddNewArticleTheme method");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateArticleTheme([FromBody] ArticleTheme articleTheme)
        {
            try
            {
                var result = _articleThemeService.UpdateArticleTheme(articleTheme);

                return Ok(result);
            }
            catch (ArticleThemeServiceException ex)
            {
                _logger.LogError(ex, "Exception in UpdatearticleTheme method");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Exception in UpdatearticleTheme method");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteArticleTheme(Guid id)
        {
            try
            {
                _articleThemeService.DeleteArticleTheme(id);

                return Ok("articleTheme was deleted");
            }   
            catch (ArticleThemeServiceException ex)
            {
                _logger.LogError(ex, "Exception in DeletearticleTheme method");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Exception in DeletearticleTheme method");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllArticleThemes()
        {
            try
            {
                var result = _articleThemeService.GetAllArticleThemes();

                return Ok(result);
            }
            catch (ArticleThemeServiceException ex)
            {
                _logger.LogError(ex, "Exception in GetAllarticleTheme method");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Exception in GetAllarticleTheme method");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetArticleThemeById(Guid id)
        {
            try
            {
                var result = _articleThemeService.GetArticleThemeById(id);

                return Ok(result);
            }
            catch (ArticleThemeServiceException ex)
            {
                _logger.LogError(ex, "Exception in GetArticleThemeById method");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Exception in GetArticleThemeById method");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("article/{id}")]
        public async Task<IActionResult> GetArticleThemesByArticleId(Guid id)
        {
            try
            {
                var result = _articleThemeService.GetArticleThemesByArticleId(id);

                return Ok(result);
            }
            catch (ArticleThemeServiceException ex)
            {
                _logger.LogError(ex, "Exception in GetArticleThemesByArticleId method");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Exception in GetArticleThemesByArticleId method");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
