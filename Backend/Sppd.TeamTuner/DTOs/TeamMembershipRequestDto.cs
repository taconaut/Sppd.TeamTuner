using System;
using System.ComponentModel.DataAnnotations;

using Sppd.TeamTuner.Core;

namespace Sppd.TeamTuner.DTOs
{
    public class TeamMembershipRequestDto
    {
        /// <summary>
        ///     The team identifier
        /// </summary>
        [Required]
        public Guid TeamId { get; set; }

        /// <summary>
        ///     The user identifier
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        ///     The comment
        /// </summary>
        [StringLength(CoreConstants.StringLength.TeamMembershipRequest.COMMENT)]
        public string Comment { get; set; }
    }
}