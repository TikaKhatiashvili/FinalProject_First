namespace HangingGame.Business;
public class HangmanGame : IGameLogic
{
    private readonly string word;
    private readonly char[] hiddenWord;
    private int attempts = 6;

    public int Score { get; private set; } = 0;

    public HangmanGame(string word)
    {
        this.word = word.ToLower();
        hiddenWord = new string('_', word.Length).ToCharArray();
    }

    public void PlayLetterGuessing()
    {
        try
        {
            while (attempts > 0 && hiddenWord.Contains('_'))
            {
                Console.WriteLine($"\nWord: {new string(hiddenWord)}");
                Console.WriteLine($"Attempts left: {attempts}. Enter a letter OR full word:");

                string? input = Console.ReadLine()?.ToLower();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Enter something.");
                    continue;
                }

                if (input.Length > 1)
                {
                    if (input == word)
                    {
                        Console.WriteLine($"\nCorrect! You WIN! The word was: {word}");
                        Score += 100;
                        return;
                    }
                    else
                    {
                        Console.WriteLine($"\nWrong full word! You lost! The word was: {word}");
                        return;
                    }
                }

                char guess = input[0];

                if (word.Contains(guess))
                {
                    for (int i = 0; i < word.Length; i++)
                    {
                        if (word[i] == guess)
                            hiddenWord[i] = guess;
                    }

                    Console.WriteLine($"Correct! Letter '{guess}' revealed.");
                    Score += 10;

                    if (!hiddenWord.Contains('_'))
                    {
                        Console.WriteLine($"\nYou revealed the word: {word}");
                        Score += 50;
                        return;
                    }
                }
                else
                {
                    attempts--;
                    Console.WriteLine($"Wrong guess! Letter '{guess}' is not in the word.");
                }
            }

            if (attempts == 0)
                Console.WriteLine($"\nYou lost! The word was: {word}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n Error occurred: {ex.Message}");
        }
    }
}