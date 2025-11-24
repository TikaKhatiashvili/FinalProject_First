namespace HangingGame.Business
{
    public class WordProvider : IWordProvider
    {
        private List<string> words = new List<string>
    {
        "monkey", "bear", "hippopotamus", "panda", "koala "
    };

        private readonly Random random = new Random();

        public string GetRandomWord()
        {
            return words[random.Next(words.Count)];
        }
    }

}
