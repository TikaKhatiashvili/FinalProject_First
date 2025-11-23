using ATMProject.Models;

namespace ATMProject.Business.IServices;
public interface IUserService
{
    Task<User> RegisterAsync(string firstName, string lastName, string personalId);
    Task<User?> AuthenticateAsync(string personalId, string pin);
}
