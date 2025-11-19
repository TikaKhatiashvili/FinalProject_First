namespace HangingGame.Business
{
    public class HangmanGame : IGameLogic
    {
        private readonly string word;
        private char[] hiddenWord;
        private int trying;

        public HangmanGame(string word)
        {
            this.word = word.ToLower();
            this.hiddenWord = new string('_', word.Length).ToCharArray();
            this.trying = 6;
        }

        public void PlayLetterGuessing()
        {
            while (trying > 0 && hiddenWord.Contains('_'))
            {
                Console.WriteLine($"Word: {new string(hiddenWord)}");
                Console.WriteLine($"You have {trying} attempts left. Enter a letter:");
                string input = Console.ReadLine().ToLower();

                if (string.IsNullOrWhiteSpace(input) || input.Length != 1)
                {
                    Console.WriteLine("Enter only one letter.");
                    continue;
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
                }
                else
                {
                    trying--;
                    Console.WriteLine($"Wrong guess! Letter '{guess}' is not in the word.");
                }

                if (!hiddenWord.Contains('_'))
                {
                    Console.WriteLine($"Congratulations! You revealed the word: {word}");
                    return;
                }
            }

            if (trying == 0)
            {
                Console.WriteLine($"You ran out of attempts. The word was: {word}");
            }
        }

        public void PlayFullWordGuessing()
        {
            Console.WriteLine("Try to guess the full word:");
            string input = Console.ReadLine().ToLower();

            if (input == word)
            {
                Console.WriteLine("Congratulations! You guessed the word!");
            }
            else
            {
                Console.WriteLine($"Wrong! The correct word was: {word}");
            }
        }
    }
}
