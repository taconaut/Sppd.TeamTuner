using System;

namespace Sppd.TeamTuner.DTOs
{
    public class TeamResponseDto : DescriptiveDto
    {
        /// <summary>
        ///     The federation identifier
        /// </summary>
        public Guid? FederationId { get; set; }
    }
}