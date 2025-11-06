using FinanceManager.Services;

namespace FinanceManager.Middleware
{
    public class VerifyToken
    {
        private RequestDelegate _next;
        private JwtService _jwtService;

        public VerifyToken(RequestDelegate next, JwtService jwtService)
        {
            _next = next;
            _jwtService = jwtService;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {

            httpContext.Request.Cookies.TryGetValue("jwt", out var token);
            if(token == null)
            {
                throw new UnauthorizedAccessException("No token provided");
            }

            var claims = _jwtService.ValidateToken(token);
            if(claims == null)
            {
                throw new UnauthorizedAccessException("Invalid token");
            }

            httpContext.Items["User"] = claims;

            await _next(httpContext);
        }
    }
}
