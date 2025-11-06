using FinanceManager.Dtos.Auth;
using FinanceManager.Services.Auth;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IValidator<AuthRegisterDto> _registerValidator;
        private IValidator<AuthLoginDto> _loginValidator;
        private IAuthService<UserDto, AuthRegisterDto, AuthLoginDto> _authService;

        public AuthController(
            IValidator<AuthRegisterDto> registerValidator,
            IValidator<AuthLoginDto> loginValidator,
            IAuthService<UserDto, AuthRegisterDto, AuthLoginDto> authService)
        {
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(AuthRegisterDto authRegisterDto)
        {
            var validationResult = _registerValidator.Validate(authRegisterDto);

            if(!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            if(!await _authService.ValidateEmail(authRegisterDto.Email))
            {
                return BadRequest("Email already taken");
            }

            var userDto = await _authService.Register(authRegisterDto);

            return Ok(userDto);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(AuthLoginDto authLoginDto)
        {
            var validationResult = _loginValidator.Validate(authLoginDto);

            if(!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var userDto = await _authService.Login(authLoginDto);

            return Ok(userDto);
        }

        [HttpPost("logout")]
        public ActionResult LogOut()
        {
            _authService.LogOut();

            return Ok(new
            {
                message = "Successfully logged out"
            });
        }
    }
}
