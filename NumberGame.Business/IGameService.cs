using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberGame.Business;

public interface IGameService
{
    int GenerateNumber(string difficulty);
    bool CheckGuess(int guess, int target);
    string GetHint(int guess, int target);
}