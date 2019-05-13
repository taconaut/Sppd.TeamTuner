using System;
using System.ComponentModel.DataAnnotations;

using Sppd.TeamTuner.Core;

namespace Sppd.TeamTuner.DTOs
{
    public class TeamMembershipRequestResponseDto
    {
        /// <summary>
        ///     The entity identifier
        /// </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        ///     The user identifier
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        ///     The name of the user
        /// </summary>
        [Required, StringLength(CoreConstants.StringLength.Named.NAME_MAX, MinimumLength = CoreConstants.StringLength.Named.NAME_MIN)]
        public string UserName { get; set; }

        /// <summary>
        ///     The comment
        /// </summary>
        [StringLength(CoreConstants.StringLength.TeamMembershipRequest.COMMENT)]
        public string Comment { get; set; }
    }
}