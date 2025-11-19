using HangingGame.Business;
namespace HangingGame.Presentation;

 class Program
{
    static void Main()
    {
        IWordProvider wordProvider = new WordProvider();
        string word = wordProvider.GetRandomWord();

        IGameLogic game = new HangmanGame(word);

        Console.WriteLine("Welcome to the Hangman Game!");
        Console.WriteLine("You have 6 attempts to guess letters.");

        game.PlayLetterGuessing();

        Console.WriteLine();
        Console.WriteLine("Now try to guess the full word!");
        game.PlayFullWordGuessing();

        Console.WriteLine("Game over!");
    }
}