using NumberGame.Business;
using NumberGame.Data;

namespace NumberGame.Presentation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IGameService gameService = new GameService();
            IScoreService scoreService = new ScoreService();
            IGameSessionService gameSession = new GameSessionService(gameService, scoreService);

            int playerCount = 0;
            const int maxPlayers = 10;

            while (playerCount < maxPlayers)
            {
                Console.WriteLine($"\n Player #{playerCount + 1} ");
                Console.Write("Enter yourname: ");
                string name = Console.ReadLine();

                gameSession.PlayOnePlayer(name);

                playerCount++;

                if (playerCount >= maxPlayers)
                {
                    Console.WriteLine("\nMaximum 10 players reached. Ending game.");
                    break;
                }

                int continueChoice = 0;
                while (true)
                {
                    Console.WriteLine("\nDo you want next player to continue? 1=Yes, 2=No");
                    string contInput = Console.ReadLine();
                    if (!int.TryParse(contInput, out continueChoice) || (continueChoice != 1 && continueChoice != 2))
                    {
                        Console.WriteLine("Invalid input! Enter 1 for Yes or 2 for No.");
                        continue;
                    }
                    break;
                }

                if (continueChoice == 2)
                    break;
            }

            Console.WriteLine("\nThanks for playing! Press any key to exit.");
            Console.ReadKey();
        }
    }
}