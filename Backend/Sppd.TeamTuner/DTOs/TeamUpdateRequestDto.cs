using System;
using System.ComponentModel.DataAnnotations;

using Sppd.TeamTuner.Core;

namespace Sppd.TeamTuner.DTOs
{
    public class TeamUpdateRequestDto : UpdateRequestDto
    {
        /// <summary>
        ///     The name of the team
        /// </summary>
        [StringLength(CoreConstants.StringLength.Named.NAME_MAX, MinimumLength = CoreConstants.StringLength.Named.NAME_MIN)]
        public string Name { get; set; }

        /// <summary>
        ///     The description
        /// </summary>
        [StringLength(CoreConstants.StringLength.Descriptive.DESCRIPTION)]
        public string Description { get; set; }

        /// <summary>
        ///     The team avatar
        /// </summary>
        public byte[] Avatar { get; set; }

        /// <summary>
        ///     The federation identifier
        /// </summary>
        public Guid? FederationId { get; set; }
    }
}