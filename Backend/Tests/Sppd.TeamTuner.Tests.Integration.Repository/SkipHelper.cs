using System.Runtime.InteropServices;

namespace Sppd.TeamTuner.Tests.Integration.DataAccess
{
    /// <summary>
    ///     Helper to decide if tests should be skipped
    /// </summary>
    public static class SkipHelper
    {
        public static bool IsWindowsOS()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        }
    }
}