using System;

namespace Sppd.TeamTuner.DTOs
{
    internal class TeamResponseDto : DescriptiveDto
    {
        /// <summary>
        ///     The federation identifier
        /// </summary>
        public Guid? FederationId { get; set; }
    }
}