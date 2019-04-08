using System;
using System.Globalization;

namespace Sppd.TeamTuner.Core.Utils.Helpers
{
    /// <summary>
    ///     Offers helper methods for files.
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        ///     Gets the clean file path for the given path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>Clean file path.</returns>
        public static string GetCleanFilePath(string path)
        {
            var uri = new UriBuilder(path);
            return Uri.UnescapeDataString(uri.Path).ToLower(CultureInfo.InvariantCulture);
        }
    }
}