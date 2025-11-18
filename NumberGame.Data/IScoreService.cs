using NumberGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberGame.Data;

public interface IScoreService
{
    void SaveScore(Player player);
    List<Player> GetTop10();
}