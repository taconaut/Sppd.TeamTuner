using System.ComponentModel.DataAnnotations;

using Sppd.TeamTuner.Core;

namespace Sppd.TeamTuner.DTOs
{
    public class TeamCreateRequestDto
    {
        /// <summary>
        ///     The team name
        /// </summary>
        [Required, StringLength(CoreConstants.StringLength.Named.NAME_MAX, MinimumLength = CoreConstants.StringLength.Named.NAME_MIN)]
        public string Name { get; set; }
    }
}