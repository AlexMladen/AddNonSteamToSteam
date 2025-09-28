using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddNonSteamToSteam.Core.Models
{
    public class OperationResult
    {
        public bool Success { get; init; }
        public string? Error { get; init; }

        public static OperationResult Ok() => new() { Success = true };
        public static OperationResult Fail(string error) => new() { Success = false, Error = error };
    }
}
