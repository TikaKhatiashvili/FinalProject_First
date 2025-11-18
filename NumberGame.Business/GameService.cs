namespace NumberGame.Business;

public class GameService : IGameService
{
    private readonly Random _random = new Random();
    public int GenerateNumber(string difficulty)
    {
        difficulty = difficulty.Trim().ToLower();

        return difficulty switch
        {
            "easy" => _random.Next(1, 16),
            "medium" => _random.Next(1, 26),
            "hard" => _random.Next(1, 51),
            _ => _random.Next(1, 16)
        };
    }
    public bool CheckGuess(int guess, int target)
    {
        return guess == target;
    }

    public string GetHint(int guess, int target)
    {
        if (guess < target) return "TooLow!";
        if (guess > target) return "TooHigh!";
        return "Correct!";
    }

   
}
