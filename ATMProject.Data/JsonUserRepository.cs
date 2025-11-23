using ATMProject.Models;
using System.Text.Json;

namespace ATMProject.Data;

public class JsonUserRepository : IUserRepository
{
    private readonly string _filePath;
    private List<User> _users = new();

    private int _nextId = 1;

    private JsonUserRepository(string filePath)
    {
        _filePath = filePath;
        Load();
    }

    public static async Task<JsonUserRepository> CreateAsync(string filePath)
    {
        var repo = new JsonUserRepository(filePath);
        await repo.SaveAsync();
        return repo;
    }

    private void Load()
    {
        if (!File.Exists(_filePath)) return;

        var json = File.ReadAllText(_filePath);
        _users = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
        _nextId = _users.Any() ? _users.Max(u => u.Id) + 1 : 1;
    }

    private async Task SaveAsync()
    {
        var json = JsonSerializer.Serialize(_users, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(_filePath, json);
    }

    public async Task<int> AddAsync(User user)
    {
        user.Id = _nextId++;
        _users.Add(user);
        await SaveAsync();
        return user.Id;
    }

    public Task<User?> GetByIdAsync(int id)
        => Task.FromResult(_users.FirstOrDefault(u => u.Id == id));

    public Task<User?> GetByPersonalIdAsync(string personalId)
        => Task.FromResult(_users.FirstOrDefault(u => u.PersonalId == personalId));

    public Task<IEnumerable<User>> GetAllAsync()
        => Task.FromResult(_users.AsEnumerable());

    public async Task UpdateAsync(User user)
    {
        var index = _users.FindIndex(u => u.Id == user.Id);
        if (index != -1)
        {
            _users[index] = user;
            await SaveAsync();
        }

    }
}