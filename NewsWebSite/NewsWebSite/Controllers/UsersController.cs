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
    public class UsersController : Controller
    {
        private IUserService _userService;
        private readonly ILogger<UsersController> _logger;
        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;

        }


        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] User user)
        {
            try 
            { 
                var result = await _userService.UpdateUserAsync(user);

                return Ok(result);
            }
            catch (UserServiceException ex)
            {
                _logger.LogError(ex, "Exception in UpdateUser method");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Exception in UpdateUser method");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try 
            { 
                await _userService.DeleteUserAsync(id);

                return Ok("User was deleted");
            }
            catch (UserServiceException ex)
            {
                _logger.LogError(ex, "Exception in DeleteUser method");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Exception in DeleteUser method");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try 
            { 
                var result = await _userService.GetAllUsersAsync();

                return Ok(result);
            }
            catch (UserServiceException ex)
            {
                _logger.LogError(ex, "Exception in GetAllUsers method");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Exception in GetAllUsers method");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            try 
            { 
                var result = await _userService.GetUserByIdAsync(id);

                return Ok(result);
            }
            catch (UserServiceException ex)
            {
                _logger.LogError(ex, "Exception in GetUserById method");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Exception in GetUserById method");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
