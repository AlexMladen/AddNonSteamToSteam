using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddNonSteamToSteam.Core.Common
{
    public interface IProcessService
    {
        bool IsSteamRunning();
        void TryCloseSteam();
    }
}
