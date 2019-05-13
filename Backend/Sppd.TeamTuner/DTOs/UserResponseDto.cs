using System;
using System.ComponentModel.DataAnnotations;

using Sppd.TeamTuner.Core;
using Sppd.TeamTuner.Core.Domain.Enumerations;

namespace Sppd.TeamTuner.DTOs
{
    public class UserResponseDto : DescriptiveDto
    {
        /// <summary>
        ///     The South Park Phone Destroyer in-game user name
        /// </summary>
        [Required, StringLength(CoreConstants.StringLength.TeamTunerUser.SPPD_NAME_MAX, MinimumLength = CoreConstants.StringLength.TeamTunerUser.SPPD_NAME_MIN)]
        public string SppdName { get; set; }

        /// <summary>
        ///     The email address
        /// </summary>
        [Required, StringLength(CoreConstants.StringLength.TeamTunerUser.EMAIL)]
        public string Email { get; set; }

        /// <summary>
        ///     The application role the current user has
        /// </summary>
        [Required, StringLength(CoreConstants.StringLength.TeamTunerUser.APPLICATION_ROLE)]
        public string ApplicationRole { get; set; }

        /// <summary>
        ///     The team identifier
        /// </summary>
        public Guid? TeamId { get; set; }

        /// <summary>
        ///     The team role
        /// </summary>
        public string TeamRole { get; set; }

        /// <summary>
        ///     The federation identifier
        /// </summary>
        public Guid? FederationId { get; set; }

        /// <summary>
        ///     The federation role
        /// </summary>
        [StringLength(CoreConstants.StringLength.TeamTunerUser.FEDERATION_ROLE)]
        public string FederationRole { get; set; }

        /// <summary>
        ///     The profile visibility determining who will be able to see the user profile
        /// </summary>
        [Required]
        public UserProfileVisibility ProfileVisibility { get; set; }
    }
}