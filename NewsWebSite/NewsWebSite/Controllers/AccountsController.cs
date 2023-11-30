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
    public class AccountsController : Controller
    {
        private IAccountService _accountService;
        private ILogger<AccountsController> _logger;
        public AccountsController(IAccountService accountService, ILogger<AccountsController> logger)
        {
            _accountService = accountService;
            _logger = logger;

        }

        [HttpPut]
        public async Task<IActionResult> UpdateAccount([FromBody] Account country)
        {
            try 
            { 
                var result = _accountService.UpdateAccount(country);

                return Ok(result);
            }
            catch (AccountServiceException ex)
            {
                _logger.LogError(ex, "Exception in UpdateAccount method");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Exception in UpdateAccount method");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(Guid id)
        {
            try 
            { 
                _accountService.DeleteAccount(id);

                return Ok("Account was deleted");
            }
            catch (AccountServiceException ex)
            {
                _logger.LogError(ex, "Exception in DeleteAccount method");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Exception in DeleteAccount method");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllAccounts()
        {
            try 
            { 
                var result = _accountService.GetAllAccounts();

                return Ok(result);
            }
            catch (AccountServiceException ex)
            {
                _logger.LogError(ex, "Exception in GetAllAccounts method");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Exception in GetAllAccounts method");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccountById(Guid id)
        {
            try 
            { 
                var result = await _accountService.GetAccountByIdAsync(id);

                return Ok(result);
            }
            catch (AccountServiceException ex)
            {
                _logger.LogError(ex, "Exception in GetAccountById method");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Exception in GetAccountById method");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpPatch("subscribe")]
        public async Task<IActionResult> Subscribe([FromBody] SubscribeModel subModel)
        {
            try
            {
                _accountService.Subscribe(subModel.authorId, subModel.subscriberId);

                return Ok();
            }
            catch (AccountServiceException ex)
            {
                _logger.LogError(ex, "Exception in Subscribe method");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Exception in Subscribe method");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
