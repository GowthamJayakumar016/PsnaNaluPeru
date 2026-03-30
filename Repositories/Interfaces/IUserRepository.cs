using Happy.Models;

namespace Happy.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<User?> GetUserByIdAsync(int id);
        Task AddUserAsync(User user);
        Task SaveAsync();
    }
}
