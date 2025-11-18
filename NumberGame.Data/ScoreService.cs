using NumberGame.Models;
using System.Numerics;

namespace NumberGame.Data;
public class ScoreService : IScoreService
{
    private readonly string _filePath = "scores.csv";

    public void SaveScore(Player player)
    {
        try
        {
            var players = LoadScores();

            var existing = players.FirstOrDefault(x => x.Id == player.Id);

            if (existing == null)
                players.Add(player);
            else if (player.BestScore > existing.BestScore)
            {
                existing.BestScore = player.BestScore;
                existing.Guesses = player.Guesses;
            }

            WriteCsv(players);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving score: {ex.Message}");
        }
    }

    private List<Player> LoadScores()
    {
        var list = new List<Player>();

        try
        {
            if (!File.Exists(_filePath))
                return list;

            var lines = File.ReadAllLines(_filePath);

            if (lines.Length <= 1) return list;

            foreach (var line in lines.Skip(1))
            {
                var parts = line.Split(',');
                if (parts.Length < 4) continue;

                list.Add(new Player
                {
                    Id = Guid.Parse(parts[0]),
                    Name = parts[1],
                    BestScore = int.Parse(parts[2]),
                    Guesses = parts[3].Split('|').Select(int.Parse).ToList()
                });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading scores: {ex.Message}");
        }

        return list;
    }

    private void WriteCsv(List<Player> players)
    {
        try
        {
            var lines = new List<string> { "Id,Name,BestScore,Guesses" };
            foreach (var p in players)
                lines.Add($"{p.Id},{p.Name},{p.BestScore},{string.Join("|", p.Guesses)}");

            File.WriteAllLines(_filePath, lines);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing CSV: {ex.Message}");
        }
    }

    public List<Player> GetTop10()
    {
        try
        {
            return LoadScores().OrderByDescending(p => p.BestScore).Take(10).ToList();
        }
        catch
        {
            return new List<Player>();
        }
    }
}
