using System;

namespace Sppd.TeamTuner.Core.Domain.Objects
{
    public class SystemInfo
    {
        public Version Version { get; set; }

        public string GitCommitHash { get; set; }

        public DateTime? BuildTimeUtc { get; set; }
    }
}