﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsWebSite_BLL.Exceptions;
using NewsWebSite_BLL.Interfaces;
using NewsWebSite_BLL.Models;

namespace NewsWebSite.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ArticleController : Controller
    {
        private IArticleService _articleService;
        private readonly ILogger<ArticleController> _logger;
        public ArticleController(IArticleService articleService, ILogger<ArticleController> logger)
        {
            _articleService = articleService;
            _logger = logger;

        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddNewArticle([FromBody] Article article)
        {
            try
            {
                var result = _articleService.AddNewArticle(article);

                return Ok(result);
            }
            catch (ArticleServiceException ex)
            {
                _logger.LogError(ex, "Exception in AddNewArticle method");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Exception in AddNewArticle method");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateArticle([FromBody] Article article)
        {
            try
            {
                var result = _articleService.UpdateArticle(article);

                return Ok(result);
            }
            catch (ArticleServiceException ex)
            {
                _logger.LogError(ex, "Exception in UpdateArticle method");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Exception in UpdateArticle method");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(Guid id)
        {
            try
            {
                _articleService.DeleteArticle(id);

                return Ok("Article was deleted");
            }
            catch (ArticleServiceException ex)
            {
                _logger.LogError(ex, "Exception in DeleteArticle method");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Exception in DeleteArticle method");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllArticles()
        {
            try
            {
                var result = _articleService.GetAllArticles();

                return Ok(result);
            }
            catch (ArticleServiceException ex)
            {
                _logger.LogError(ex, "Exception in GetAllArticle method");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Exception in GetAllArticle method");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetArticleById(Guid id)
        {
            try
            {
                var result = _articleService.GetArticleById(id);

                return Ok(result);
            }
            catch (ArticleServiceException ex)
            {
                _logger.LogError(ex, "Exception in GetArticleById method");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Exception in GetArticleById method");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("accountid/{id}")]
        public async Task<IActionResult> GetArticleByAccountId(Guid id)
        {
            try
            {
                var result = _articleService.GetArticlesByAccountId(id);

                return Ok(result);
            }
            catch (ArticleServiceException ex)
            {
                _logger.LogError(ex, "Exception in GetArticleByAccountId method");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Exception in GetArticleByAccountId method");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateArticleThemes([FromRoute] Guid id, [FromBody] Guid[] themeIds)
        {
            try
            {
                var result = _articleService.ChangeArticleThemes(id, themeIds);

                return Ok(result);
            }
            catch (ArticleServiceException ex)
            {
                _logger.LogError(ex, "Exception in UpdateArticle method");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Exception in UpdateArticle method");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("theme/{id}")]
        public async Task<IActionResult> GetArticleByThemeId(Guid id)
        {
            try
            {
                var result = _articleService.GetArticlesByThemeId(id);

                return Ok(result);
            }
            catch (ArticleServiceException ex)
            {
                _logger.LogError(ex, "Exception in GetArticleByThemeId method");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Exception in GetArticleByThemeId method");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


    }
}
