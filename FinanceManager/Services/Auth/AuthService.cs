using FinanceManager.Dtos.Auth;
using FinanceManager.Exceptions;
using FinanceManager.Mappers.Auth;
using FinanceManager.Models;
using FinanceManager.Repository.Auth;

namespace FinanceManager.Services.Auth
{
    public class AuthService : IAuthService<UserDto, AuthRegisterDto, AuthLoginDto>
    {
        private IAuthRepository<User> _authRepository;
        private JwtService _jwtService;
        private IHttpContextAccessor _httpContextAccessor;

        public AuthService(IAuthRepository<User> authRepository, JwtService jwtService, IHttpContextAccessor httpContextAccessor)
        {
            _authRepository = authRepository;
            _jwtService = jwtService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UserDto> Register(AuthRegisterDto authRegisterDto)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(authRegisterDto.Password);

            var user = authRegisterDto.ToUser(hashedPassword);

            await _authRepository.Add(user);
            await _authRepository.Save();

            var userDto = user.ToUserDto();

            return userDto;
        }

        public async Task<UserDto> Login(AuthLoginDto authLoginDto)
        {
            var user = await _authRepository.GetUserByEmail(authLoginDto.Email);
            if(user == null)
            {
                throw new NotFoundException("The email is not registered");
            }

            var isPasswordValid = BCrypt.Net.BCrypt.Verify(authLoginDto.Password, user.HashedPassword);
            if(!isPasswordValid)
            {
                throw new UnauthorizedAccessException("Incorrect password");
            }


            var userDto = user.ToUserDto();
            var token = _jwtService.GenerateToken(userDto);

            _httpContextAccessor.HttpContext.Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddHours(2)
            });

            return userDto;
        }

        public void LogOut()
        {
            var options = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = "/"
            };
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("jwt", options);
        }

        public async Task<bool> ValidateEmail(string email)
        {
            var user = await _authRepository.GetUserByEmail(email);
            return user == null;
        }
    }
}
