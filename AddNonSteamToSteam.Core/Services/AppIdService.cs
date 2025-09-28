using System.Text;
using AddNonSteamToSteam.Core.Common;

namespace AddNonSteamToSteam.Core.Services;

public class AppIdService : IAppIdService
{
    public AppIdService()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    public uint ComputeNonSteamAppId(string appName, string exeQuoted)
    {
        // Valve convention: CRC32 over (AppName + Exe + '\0'), then OR with 0x80000000
        var combined = (appName ?? string.Empty) + (exeQuoted ?? string.Empty) + "\0";
        var bytes = Encoding.GetEncoding(1252).GetBytes(combined);
        uint crc = Crc32(bytes);
        return crc | 0x80000000;
    }

    private static uint Crc32(byte[] bytes)
    {
        const uint poly = 0xEDB88320;
        uint[] table = new uint[256];
        for (uint i = 0; i < 256; i++)
        {
            uint temp = i;
            for (int j = 0; j < 8; j++)
                temp = (temp & 1) == 1 ? (poly ^ (temp >> 1)) : (temp >> 1);
            table[i] = temp;
        }

        uint crc = 0xFFFFFFFF;
        foreach (byte b in bytes)
        {
            var idx = (byte)((crc & 0xFF) ^ b);
            crc = (crc >> 8) ^ table[idx];
        }
        return ~crc;
    }
}
