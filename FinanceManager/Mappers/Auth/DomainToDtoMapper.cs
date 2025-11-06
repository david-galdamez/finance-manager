using FinanceManager.Dtos.Auth;
using FinanceManager.Models;

namespace FinanceManager.Mappers.Auth
{
    public static class DomainToDtoMapper
    {
        public static UserDto ToUserDto(this User user)
        {
            return new UserDto
            {
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email
            };
        }
    }
}
