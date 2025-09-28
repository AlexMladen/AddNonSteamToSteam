using System.Drawing;
using System.Drawing.Imaging;
using AddNonSteamToSteam.Core.Common;
using AddNonSteamToSteam.Core.Models;

namespace AddNonSteamToSteam.Core.Services;

public class ArtworkService : IArtworkService
{
    public OperationResult SaveArtwork(
        string gridDir, uint nonSteamAppId,
        byte[]? portraitJpg, byte[]? heroJpg, byte[]? logoPng,
        byte[]? headerJpg = null)
    {
        try
        {
            Directory.CreateDirectory(gridDir);

            // Portrait -> <id>.png and <id>p.png (Big Picture vertical list uses the 'p' variant)
            if (portraitJpg is { Length: > 0 })
            {
                using var ms = new MemoryStream(portraitJpg);
                using var img = Image.FromStream(ms);
                img.Save(Path.Combine(gridDir, $"{nonSteamAppId}.png"), ImageFormat.Png);
                img.Save(Path.Combine(gridDir, $"{nonSteamAppId}p.png"), ImageFormat.Png);
            }

            if (heroJpg is { Length: > 0 })
            {
                File.WriteAllBytes(Path.Combine(gridDir, $"{nonSteamAppId}_hero.jpg"), heroJpg);
            }

            if (headerJpg is { Length: > 0 })
            {
                File.WriteAllBytes(Path.Combine(gridDir, $"{nonSteamAppId}_header.jpg"), headerJpg);
            }

            if (logoPng is { Length: > 0 })
            {
                File.WriteAllBytes(Path.Combine(gridDir, $"{nonSteamAppId}_logo.png"), logoPng);
            }

            return OperationResult.Ok();
        }
        catch (Exception ex)
        {
            return OperationResult.Fail(ex.Message);
        }
    }

    public OperationResult SaveIconFromExe(string exePath, string gridDir, uint nonSteamAppId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(exePath) || !File.Exists(exePath))
                return OperationResult.Fail("EXE not found for icon extraction.");

            using var ico = Icon.ExtractAssociatedIcon(exePath);
            if (ico == null) return OperationResult.Fail("No associated icon found.");

            using var bmp = ico.ToBitmap();
            Directory.CreateDirectory(gridDir);
            var iconPath = Path.Combine(gridDir, $"{nonSteamAppId}_icon.jpg");
            bmp.Save(iconPath, ImageFormat.Jpeg);
            return OperationResult.Ok();
        }
        catch (Exception ex)
        {
            return OperationResult.Fail(ex.Message);
        }
    }
}
