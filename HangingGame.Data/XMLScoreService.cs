using HangingGame.Models;
using System.Xml.Linq;

namespace HangingGame.Data;
public class XMLScoreService
{
    private readonly string _filePath;
    private readonly SemaphoreSlim _lock = new(1, 1);
    private readonly XName RootName = "Players";
    private readonly XName PlayerTag = "Player";

    public XMLScoreService(string filePath) => _filePath = filePath;

    private XDocument LoadOrCreate()
    {
        if (!File.Exists(_filePath))
        {
            var root = new XElement(RootName);
            var doc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), root);
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
            doc.Save(_filePath);
            return doc;
        }
        return XDocument.Load(_filePath);
    }

    private void Save(XDocument doc)
    {
        var tmp = _filePath + ".tmp";
        doc.Save(tmp);
        File.Copy(tmp, _filePath, true);
        File.Delete(tmp);
    }

    public async Task AddOrUpdateAsync(PlayerRecord record)
    {
        await _lock.WaitAsync();
        try
        {
            var doc = LoadOrCreate();

            var existing = doc.Descendants(PlayerTag)
                .FirstOrDefault(x => (string?)x.Element("Name") == record.PlayerName);

            if (existing != null)
            {
                int oldHighScore = int.TryParse((string?)existing.Element("HighScore"), out var hs) ? hs : 0;
                int newHighScore = Math.Max(oldHighScore, record.HighScore);
                existing.SetElementValue("HighScore", newHighScore);
                existing.SetElementValue("LastPlayed", record.LastPlayed.ToString("o"));
                int games = int.TryParse((string?)existing.Element("GamesPlayed"), out var gp) ? gp : 1;
                existing.SetElementValue("GamesPlayed", games + 1);
            }
            else
            {
                var el = new XElement(PlayerTag,
                    new XElement("Id", record.Id.ToString()),
                    new XElement("Name", record.PlayerName),
                    new XElement("HighScore", record.HighScore),
                    new XElement("LastPlayed", record.LastPlayed.ToString("o")),
                    new XElement("GamesPlayed", record.GamesPlayed)
                );

                doc.Root!.Add(el);
            }

            var ordered = doc.Root!.Elements(PlayerTag)
                .OrderByDescending(x => int.TryParse((string?)x.Element("HighScore"), out var hs) ? hs : 0)
                .ToList();

            doc.Root!.RemoveAll();
            doc.Root!.Add(ordered);

            Save(doc);
        }
        finally { _lock.Release(); }
    }

    public async Task<List<PlayerRecord>> GetAllAsync()
    {
        await _lock.WaitAsync();
        try
        {
            var doc = LoadOrCreate();

            return doc.Descendants(PlayerTag)
                .Select(x => new PlayerRecord
                {
                    Id = Guid.TryParse((string?)x.Element("Id"), out var guid) ? guid : Guid.NewGuid(),
                    PlayerName = (string?)x.Element("Name") ?? "Unknown",
                    HighScore = int.TryParse((string?)x.Element("HighScore"), out var score) ? score : 0,
                    LastPlayed = DateTime.TryParse((string?)x.Element("LastPlayed"), out var dt) ? dt : DateTime.UtcNow,
                    GamesPlayed = int.TryParse((string?)x.Element("GamesPlayed"), out var games) ? games : 1
                })
                .OrderByDescending(x => x.HighScore) 
                .ToList();
        }
        finally { _lock.Release(); }
    }
}