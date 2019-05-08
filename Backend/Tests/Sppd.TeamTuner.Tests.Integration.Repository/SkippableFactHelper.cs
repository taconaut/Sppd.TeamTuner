using System.Collections.Generic;

using Xunit;

namespace Sppd.TeamTuner.Tests.Integration.Repository
{
    /// <summary>
    ///     Helper for <see cref="SkippableFactAttribute" />
    /// </summary>
    public static class SkippableFactHelper
    {
        private static readonly List<string> s_noneRelationalDatabaseProviders = new List<string> {"InMemory"};

        /// <summary>
        ///     All providers except InMemory are considered to be relational.
        /// </summary>
        /// <param name="provider"></param>
        public static bool IsRelationalDatabase(string provider)
        {
            return !s_noneRelationalDatabaseProviders.Contains(provider);
        }
    }
}