using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangingGame.Models;
public class PlayerRecord
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string PlayerName { get; set; } = string.Empty;
    public int HighScore { get; set; } = 0;
    public int GamesPlayed { get; set; } = 1;
    public DateTime LastPlayed { get; set; } = DateTime.UtcNow;
}