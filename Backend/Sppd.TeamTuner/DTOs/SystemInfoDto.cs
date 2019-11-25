using System;
using System.ComponentModel.DataAnnotations;

namespace Sppd.TeamTuner.DTOs
{
    public class SystemInfoDto
    {
        [Required]
        public string Version { get; set; }

        public string GitCommitHash { get; set; }

        public DateTime? BuildTimeUtc { get; set; }
    }
}