using Microsoft.AspNetCore.Mvc;
using StudentManagement.Core.Interfaces.Services;
using StudentManagement.Core.ViewModel;
using StudentManagement.Infrastructure.Services;

namespace StudentManagement.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService) 
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            try
            {
                return Ok(await _authService.Register(viewModel));
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            try
            {
                var user = await _authService.Login(viewModel);
                return Ok(user);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            return Ok(await _authService.Logout());
        }
    }
}
