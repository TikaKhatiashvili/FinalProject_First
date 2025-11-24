using HangingGame.Business;
using HangingGame.Data;
using HangingGame.Models;
namespace HangingGame.Presentation;

 class Program
{
    static async Task Main()
    {
        Console.Write("Enter your name: ");
        string name = Console.ReadLine()!;

        XMLScoreService xml = new XMLScoreService("Data/Players.xml");

        IWordProvider provider = new WordProvider();
        string word = provider.GetRandomWord();

        IGameLogic game = new HangmanGame(word);

        Console.WriteLine("Welcome to Hangman!");

        game.PlayLetterGuessing();

        var record = new PlayerRecord
        {
            Id = Guid.NewGuid(),
            PlayerName = name,
            HighScore = ((HangmanGame)game).Score,
            LastPlayed = DateTime.UtcNow,
            GamesPlayed = 1
        };

        await xml.AddOrUpdateAsync(record);

      
        Console.WriteLine("\n   TOP 10 PLAYERS  ");
        var top = await xml.GetAllAsync();

        foreach (var p in top)
            Console.WriteLine($"{p.PlayerName} - HighScore: {p.HighScore} - Games: {p.GamesPlayed}");
    }
}
