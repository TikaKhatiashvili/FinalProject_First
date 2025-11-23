using ATMProject.Business.IServices;
using ATMProject.Data;
using ATMProject.Models;

namespace ATMProject.Business.ServicesImplementations;
public class UserService : IUserService
{
    private readonly IUserRepository _userRepo;
    private readonly Random _random = new();

    public UserService(IUserRepository userRepo)
    {
        _userRepo = userRepo;
    }

    public async Task<User> RegisterAsync(string firstName, string lastName, string personalId)
    {
        if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentException("Name is required..");
        if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentException("Last name is required.");
        if (string.IsNullOrWhiteSpace(personalId)) throw new ArgumentException("Personal number is required.");

        var existing = await _userRepo.GetByPersonalIdAsync(personalId);
        if (existing != null) throw new InvalidOperationException("The user already exists.");

        var allUsers = await _userRepo.GetAllAsync();
        var usedPins = allUsers.Select(u => u.PIN).ToHashSet();

        string pin;
        int attempts = 0;
        do
        {
            pin = _random.Next(1000, 10000).ToString();
            attempts++;
            if (attempts > 10000) throw new InvalidOperationException("PIN generation failed..");
        } while (usedPins.Contains(pin));

        var user = new User
        {
            FirstName = firstName.Trim(),
            LastName = lastName.Trim(),
            PersonalId = personalId.Trim(),
            PIN = pin,
            Balance = 0
        };

        var id = await _userRepo.AddAsync(user);
        var persisted = await _userRepo.GetByIdAsync(id);
        if (persisted == null) throw new InvalidOperationException("Failed to save user.");
        return persisted;
    }

    public async Task<User?> AuthenticateAsync(string personalId, string pin)
    {
        if (string.IsNullOrWhiteSpace(personalId) || string.IsNullOrWhiteSpace(pin))
            return null;

        var user = await _userRepo.GetByPersonalIdAsync(personalId);
        if (user == null) return null;
        return user.PIN == pin ? user : null;
    }
}