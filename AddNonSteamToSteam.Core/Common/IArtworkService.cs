using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddNonSteamToSteam.Core.Common
{
    public interface IArtworkService
    {
        Models.OperationResult SaveArtwork(
            string gridDir, uint nonSteamAppId,
            byte[]? portraitJpg, byte[]? heroJpg, byte[]? logoPng,
            byte[]? headerJpg = null);

        Models.OperationResult SaveIconFromExe(string exePath, string gridDir, uint nonSteamAppId);

    }
}
