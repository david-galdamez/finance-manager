using FinanceManager.Dtos.Auth;
using FinanceManager.Models;

namespace FinanceManager.Mappers.Auth
{
    public static class DtoToDomainMapper
    {
        public static User ToUser(this AuthRegisterDto dto, string hashedPassword)
        {
            return new User
            {
                Name = dto.Name,
                Email = dto.Email,
                HashedPassword = hashedPassword
            };
        }
    }
}
