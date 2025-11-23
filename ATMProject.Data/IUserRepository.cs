using ATMProject.Models;

namespace ATMProject.Data;

public interface IUserRepository
{
    Task<int> AddAsync(User user);
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByPersonalIdAsync(string personalId);
    Task<IEnumerable<User>> GetAllAsync();
    Task UpdateAsync(User user);
}
