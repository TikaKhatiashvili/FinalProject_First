namespace NumberGame.Models;

public class Player
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int BestScore { get; set; }
    public List<int> Guesses { get; set; } = new();
}
