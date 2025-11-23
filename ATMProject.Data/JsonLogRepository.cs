using ATMProject.Models;
using System.Text.Json;

namespace ATMProject.Data;
public class JsonLogRepository : ILogRepository
{
    private readonly string _filePath;
    private List<TransactionLog> _logs = new();

    private JsonLogRepository(string filePath)
    {
        _filePath = filePath;
        Load();
    }

    public static async Task<JsonLogRepository> CreateAsync(string filePath)
    {
        var repo = new JsonLogRepository(filePath);
        await repo.SaveAsync();
        return repo;
    }

    private void Load()
    {
        if (!File.Exists(_filePath)) return;

        var json = File.ReadAllText(_filePath);
        _logs = JsonSerializer.Deserialize<List<TransactionLog>>(json) ?? new List<TransactionLog>();
    }

    private async Task SaveAsync()
    {
        var json = JsonSerializer.Serialize(_logs, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(_filePath, json);
    }

    public async Task AddLogAsync(TransactionLog log)
    {
        _logs.Add(log);
        await SaveAsync();
    }

    public Task<IEnumerable<TransactionLog>> GetAllLogsAsync()
        => Task.FromResult(_logs.AsEnumerable());
}
