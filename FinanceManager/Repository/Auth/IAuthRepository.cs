using FinanceManager.Models;

namespace FinanceManager.Repository.Auth
{
    public interface IAuthRepository<TEntity>
    {
        Task<TEntity> GetUserByEmail(string email);
        Task Add(TEntity entity);
        Task Save();
    }
}
