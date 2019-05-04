using System.Reflection;

namespace Sppd.TeamTuner.Core.Utils.Extensions
{
    /// <summary>
    ///     Extension methods for <see cref="Assembly" />
    /// </summary>
    public static class AssemblyExtensions
    {
        /// <summary>
        ///     Gets the file path of the assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>The assembly file path.</returns>
        public static string GetFilePath(this Assembly assembly)
        {
            return assembly?.Location;
        }
    }
}