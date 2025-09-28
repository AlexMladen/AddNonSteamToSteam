using AddNonSteamToSteam.Core.Common;

namespace AddNonSteamToSteam.Core.Services;

public class UrlService : IUrlService
{
    public string ReadUriFromUrlFile(string filePath)
    {
        if (!File.Exists(filePath)) throw new FileNotFoundException(filePath);
        foreach (var line in File.ReadAllLines(filePath))
        {
            if (line.StartsWith("URL=", StringComparison.OrdinalIgnoreCase))
                return line.Substring(4).Trim();
        }
        throw new InvalidOperationException("URL= line not found in .url file.");
    }
}
