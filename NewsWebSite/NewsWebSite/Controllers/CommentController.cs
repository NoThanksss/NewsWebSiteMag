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
    public class CommentController : Controller
    {
        private ICommentService _commentService;
        private readonly ILogger<CommentController> _logger;
        public CommentController(ICommentService commentService, ILogger<CommentController> logger)
        {
            _commentService = commentService;
            _logger = logger;

        }

        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> AddNewComment([FromBody] Comment Comment)
        {
            try
            {
                var result = await _commentService.AddNewCommentAsync(Comment);

                return Ok(result);
            }
            catch (CommentServiceException ex)
            {
                _logger.LogError(ex, "Exception in AddNewComment method");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Exception in AddNewComment method");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateComment([FromBody] Comment Comment)
        {
            try
            {
                var result = await _commentService.UpdateCommentAsync(Comment);

                return Ok(result);
            }
            catch (CommentServiceException ex)
            {
                _logger.LogError(ex, "Exception in UpdateComment method");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Exception in UpdateComment method");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            try
            {
                await _commentService.DeleteCommentAsync(id);

                return Ok("Comment was deleted");
            }
            catch (CommentServiceException ex)
            {
                _logger.LogError(ex, "Exception in DeleteComment method");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Exception in DeleteComment method");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllComments()
        {
            try
            {
                var result = await _commentService.GetAllCommentsAsync();

                return Ok(result);
            }
            catch (CommentServiceException ex)
            {
                _logger.LogError(ex, "Exception in GetAllComment method");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Exception in GetAllComment method");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommentById(Guid id)
        {
            try
            {
                var result = await _commentService.GetCommentByIdAsync(id);

                return Ok(result);
            }
            catch (CommentServiceException ex)
            {
                _logger.LogError(ex, "Exception in GetCommentById method");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Exception in GetCommentById method");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("article/{id}")]
        public async Task<IActionResult> GetCommentsByArticleId(Guid id)
        {
            try
            {
                var result = await _commentService.GetCommentsByArticleIdAsync(id);

                return Ok(result);
            }
            catch (CommentServiceException ex)
            {
                _logger.LogError(ex, "Exception in GetCommentsByArticleId method");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Exception in GetCommentsByArticleId method");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
