using System.Diagnostics;
using AddNonSteamToSteam.Core.Common;

namespace AddNonSteamToSteam.Core.Services;

public class ProcessService : IProcessService
{
    public bool IsSteamRunning() => Process.GetProcessesByName("steam").Any();

    public void TryCloseSteam()
    {
        foreach (var p in Process.GetProcessesByName("steam"))
        {
            try { p.CloseMainWindow(); p.WaitForExit(2000); } catch { }
            try { if (!p.HasExited) p.Kill(true); } catch { }
        }
    }
}
