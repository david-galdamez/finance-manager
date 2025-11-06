using FinanceManager.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Repository.Auth
{
    public class AuthRepository : IAuthRepository<User>
    {
        private StoreContext _context;

        public AuthRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByEmail(string email) =>
            await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        public async Task Add(User entity) =>
            await _context.Users.AddAsync(entity);

        public async Task Save() =>
            await _context.SaveChangesAsync();
    }
}
