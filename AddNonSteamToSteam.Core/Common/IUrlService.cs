using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddNonSteamToSteam.Core.Common
{
    public interface IUrlService
    {
        /// <summary>Reads the URL=… line from a .url file (INI-like) and returns the Epic/other URI.</summary>
        string ReadUriFromUrlFile(string filePath);
    }
}
