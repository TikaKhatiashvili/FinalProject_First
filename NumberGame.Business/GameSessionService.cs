using NumberGame.Data;
using NumberGame.Models;

namespace NumberGame.Business;

public class GameSessionService : IGameSessionService
{
    private readonly IGameService _gameService;
    private readonly IScoreService _scoreService;

    public GameSessionService(IGameService gameService, IScoreService scoreService)
    {
        _gameService = gameService;
        _scoreService = scoreService;
    }

    public void PlayOnePlayer(string playerName)
    {
        Guid playerId = Guid.NewGuid();
        int difficultyChoice = 0;

        while (true)
        {
            Console.WriteLine("Choose difficulty: 1=Easy, 2=Medium, 3=Hard");
            string input = Console.ReadLine();
            if (!int.TryParse(input, out difficultyChoice) || difficultyChoice < 1 || difficultyChoice > 3)
            {
                Console.WriteLine("Invalid input! Enter 1, 2, or 3.");
                continue;
            }
            break;
        }

        string difficulty = difficultyChoice == 1 ? "easy" :
                    difficultyChoice == 2 ? "medium" :
                    difficultyChoice == 3 ? "hard" : "easy";


        int targetNumber = _gameService.GenerateNumber(difficulty);
        List<int> guesses = new List<int>();
        int score = 0;
        int attempts = 10;

        for (int i = 1; i <= attempts; i++)
        {
            Console.Write($"Attempt {i}/{attempts} - Enter your guess: ");
            if (!int.TryParse(Console.ReadLine(), out int guess))
            {
                Console.WriteLine("Invalid input. Enter a number.");
                i--;
                continue;
            }

            guesses.Add(guess);

            if (_gameService.CheckGuess(guess, targetNumber))
            {
                score = attempts - i + 1;
                Console.WriteLine($"Correct! Your score: {score}");
                break;
            }

            Console.WriteLine(_gameService.GetHint(guess, targetNumber));
        }

        var player = new Player
        {
            Id = playerId,
            Name = playerName,
            BestScore = score,
            Guesses = guesses
        };

        _scoreService.SaveScore(player);

        Console.WriteLine("\nYour score saved!");
        Console.WriteLine("Top 10 Players:");
        foreach (var p in _scoreService.GetTop10())
            Console.WriteLine($"{p.Name} - Best Score: {p.BestScore}");
    }
}