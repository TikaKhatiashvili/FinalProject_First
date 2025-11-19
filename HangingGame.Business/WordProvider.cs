using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangingGame.Business
{
    public class WordProvider : IWordProvider
    {
        private List<string> words = new List<string>
    {
        "monkey", "bear", "hippopotamus", "panda", "koala "
    };

        private Random random = new Random();

        public string GetRandomWord()
        {
            int index = random.Next(words.Count);
            return words[index];
        }
    }

}
