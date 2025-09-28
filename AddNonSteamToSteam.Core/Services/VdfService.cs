using System.Text;
using AddNonSteamToSteam.Core.Common;
using AddNonSteamToSteam.Core.Models;

namespace AddNonSteamToSteam.Core.Services;

public class VdfService : IVdfService
{
    public IReadOnlyList<ShortcutModel> ReadShortcuts(string shortcutsVdfPath)
    {
        if (!File.Exists(shortcutsVdfPath)) return new List<ShortcutModel>();
        using var fs = File.OpenRead(shortcutsVdfPath);
        using var br = new BinaryReader(fs, Encoding.UTF8);

        byte rootType = br.ReadByte();
        string rootKey = ReadCString(br);
        if (rootType != 0x00 || rootKey != "shortcuts")
            throw new Exception("Invalid shortcuts.vdf (root).");

        var list = new List<ShortcutModel>();
        while (true)
        {
            byte t = br.ReadByte();
            if (t == 0x08) break;           // end root
            if (t != 0x00) throw new Exception($"Expected dict, got 0x{t:X2}");
            _ = ReadCString(br);            // index "0","1",...
            list.Add(ParseShortcut(br));
        }
        return list;
    }

    public Models.OperationResult WriteShortcuts(string shortcutsVdfPath, IEnumerable<ShortcutModel> shortcuts, bool backup = true)
    {
        try
        {
            if (backup && File.Exists(shortcutsVdfPath))
            {
                var backupPath = shortcutsVdfPath + ".backup_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                File.Copy(shortcutsVdfPath, backupPath, true);
            }

            using var fs = new FileStream(shortcutsVdfPath, FileMode.Create, FileAccess.Write);
            using var bw = new BinaryWriter(fs, Encoding.UTF8);

            // root
            bw.Write((byte)0x00); WriteCStr(bw, "shortcuts");

            int i = 0;
            foreach (var sc in shortcuts)
            {
                bw.Write((byte)0x00); WriteCStr(bw, i.ToString());
                WriteShortcut(bw, sc);
                bw.Write((byte)0x08); // end of this shortcut dict
                i++;
            }

            // end root
            bw.Write((byte)0x08);
            bw.Write((byte)0x08); // Steam writes an extra 0x08
            return Models.OperationResult.Ok();
        }
        catch (Exception ex)
        {
            return Models.OperationResult.Fail(ex.Message);
        }
    }

    // ---- parsing helpers ----
    private static ShortcutModel ParseShortcut(BinaryReader br)
    {
        var sc = new ShortcutModel();

        while (true)
        {
            byte t = br.ReadByte();
            if (t == 0x08) break; // end of dict
            string key = ReadCString(br);

            switch (t)
            {
                case 0x00: // nested dict (tags)
                    if (key == "tags") sc.Tags = ParseTags(br); else SkipDict(br);
                    break;

                case 0x01: // string
                    var s = ReadCString(br);
                    switch (key)
                    {
                        case "AppName": sc.AppName = s; break;
                        case "Exe": sc.Exe = s; break;
                        case "StartDir": sc.StartDir = s; break;
                        case "icon": sc.Icon = s; break;
                        case "ShortcutPath": sc.ShortcutPath = s; break;
                        case "LaunchOptions": sc.LaunchOptions = s; break;
                        case "DevkitGameID": sc.DevkitGameID = s; break;
                        case "FlatpakAppID": sc.FlatpakAppID = s; break;
                    }
                    break;

                case 0x02: // int32
                    int i = br.ReadInt32();
                    switch (key)
                    {
                        case "appid": sc.NonSteamAppId = unchecked((uint)i); break;
                        case "IsHidden": sc.IsHidden = i != 0; break;
                        case "AllowDesktopConfig": sc.AllowDesktopConfig = i != 0; break;
                        case "AllowOverlay": sc.AllowOverlay = i != 0; break;
                        case "OpenVR": sc.OpenVR = i != 0; break;
                        case "Devkit": sc.Devkit = i != 0; break;
                            // DevkitOverrideAppID, LastPlayTime ignored
                    }
                    break;

                default:
                    throw new Exception($"Unknown type 0x{t:X2}");
            }
        }
        return sc;
    }

    private static List<string> ParseTags(BinaryReader br)
    {
        var tags = new List<string>();
        while (true)
        {
            byte t = br.ReadByte();
            if (t == 0x08) break;
            string k = ReadCString(br);
            if (t != 0x01) throw new Exception("Unexpected tag type");
            string v = ReadCString(br);
            tags.Add(v);
        }
        return tags;
    }

    private static void SkipDict(BinaryReader br)
    {
        while (true)
        {
            byte t = br.ReadByte();
            if (t == 0x08) break;
            string _ = ReadCString(br);
            switch (t)
            {
                case 0x00: SkipDict(br); break;
                case 0x01: ReadCString(br); break;
                case 0x02: br.ReadInt32(); break;
                default: throw new Exception($"Unknown type 0x{t:X2}");
            }
        }
    }

    private static string ReadCString(BinaryReader br)
    {
        var bytes = new List<byte>();
        while (true)
        {
            byte b = br.ReadByte();
            if (b == 0x00) break;
            bytes.Add(b);
        }
        return Encoding.UTF8.GetString(bytes.ToArray());
    }

    private static void WriteShortcut(BinaryWriter bw, ShortcutModel sc)
    {
        // int appid
        bw.Write((byte)0x02); WriteCStr(bw, "appid"); bw.Write(unchecked((int)sc.NonSteamAppId));

        // strings
        WriteStr(bw, "AppName", sc.AppName);
        WriteStr(bw, "Exe", sc.Exe);
        WriteStr(bw, "StartDir", sc.StartDir);
        WriteStr(bw, "icon", sc.Icon);
        WriteStr(bw, "ShortcutPath", sc.ShortcutPath);
        WriteStr(bw, "LaunchOptions", sc.LaunchOptions);
        WriteStr(bw, "DevkitGameID", sc.DevkitGameID);
        WriteStr(bw, "FlatpakAppID", sc.FlatpakAppID);

        // ints
        WriteInt(bw, "IsHidden", sc.IsHidden ? 1 : 0);
        WriteInt(bw, "AllowDesktopConfig", sc.AllowDesktopConfig ? 1 : 0);
        WriteInt(bw, "AllowOverlay", sc.AllowOverlay ? 1 : 0);
        WriteInt(bw, "OpenVR", sc.OpenVR ? 1 : 0);
        WriteInt(bw, "Devkit", sc.Devkit ? 1 : 0);

        // tags block
        bw.Write((byte)0x00); WriteCStr(bw, "tags");
        for (int i = 0; i < sc.Tags.Count; i++)
        {
            bw.Write((byte)0x01); WriteCStr(bw, i.ToString()); WriteCStr(bw, sc.Tags[i]);
        }
        bw.Write((byte)0x08);
    }

    private static void WriteCStr(BinaryWriter bw, string s) { bw.Write(Encoding.UTF8.GetBytes(s)); bw.Write((byte)0x00); }
    private static void WriteStr(BinaryWriter bw, string key, string val) { bw.Write((byte)0x01); WriteCStr(bw, key); WriteCStr(bw, val ?? ""); }
    private static void WriteInt(BinaryWriter bw, string key, int val) { bw.Write((byte)0x02); WriteCStr(bw, key); bw.Write(val); }
}
