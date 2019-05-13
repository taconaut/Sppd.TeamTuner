using System.ComponentModel.DataAnnotations;

using Sppd.TeamTuner.Core;
using Sppd.TeamTuner.Core.Domain.Enumerations;

namespace Sppd.TeamTuner.DTOs
{
    public class UserUpdateRequestDto : UpdateRequestDto
    {
        /// <summary>
        ///     The user name
        /// </summary>
        [StringLength(CoreConstants.StringLength.TeamTunerUser.SPPD_NAME_MAX, MinimumLength = CoreConstants.StringLength.TeamTunerUser.SPPD_NAME_MIN)]
        public string Name { get; set; }

        /// <summary>
        ///     The South Park Phone Destroyer in-game user name
        /// </summary>
        [StringLength(CoreConstants.StringLength.TeamTunerUser.SPPD_NAME_MAX, MinimumLength = CoreConstants.StringLength.TeamTunerUser.SPPD_NAME_MIN)]
        public string SppdName { get; set; }

        /// <summary>
        ///     The email address
        /// </summary>
        [StringLength(CoreConstants.StringLength.TeamTunerUser.EMAIL)]
        public string Email { get; set; }

        /// <summary>
        ///     The avatar of the user
        /// </summary>
        public byte[] Avatar { get; set; }

        /// <summary>
        ///     The description
        /// </summary>
        [StringLength(CoreConstants.StringLength.Descriptive.DESCRIPTION)]
        public string Description { get; set; }

        /// <summary>
        ///     The profile visibility determining who will be able to see the user profile
        /// </summary>
        public UserProfileVisibility ProfileVisibility { get; set; }
    }
}