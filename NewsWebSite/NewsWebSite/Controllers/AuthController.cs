using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MailKit.Net.Smtp;
using NewsWebSite_BLL.Interfaces;
using NewsWebSite_BLL.Models;

namespace NewsWebSite.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger, IAuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            return Ok(await _authService.RegisterAsync(model));

        }

        [HttpPost("LogIn")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            return Ok(await _authService.LoginAsync(model));

        }
    }
}
