using FinanceManager.Dtos.Auth;

namespace FinanceManager.Services.Auth
{
    public interface IAuthService<T, Tr, Tl>
    {
        Task<T> Register(Tr authRegisterDto);
        Task<T> Login(Tl authLoginDto);
        void LogOut();

        Task<bool> ValidateEmail(string email);
    }
}
