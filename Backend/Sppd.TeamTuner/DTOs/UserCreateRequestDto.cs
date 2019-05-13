using System.ComponentModel.DataAnnotations;

using Sppd.TeamTuner.Authorization;
using Sppd.TeamTuner.Core;

namespace Sppd.TeamTuner.DTOs
{
    public class UserCreateRequestDto
    {
        /// <summary>
        ///     The user name
        /// </summary>
        [Required, StringLength(CoreConstants.StringLength.Named.NAME_MAX, MinimumLength = CoreConstants.StringLength.Named.NAME_MIN)]
        public string Name { get; set; }

        /// <summary>
        ///     The South Park Phone Destroyer in-game user name
        /// </summary>
        [Required, StringLength(CoreConstants.StringLength.TeamTunerUser.SPPD_NAME_MAX, MinimumLength = CoreConstants.StringLength.TeamTunerUser.SPPD_NAME_MIN)]
        public string SppdName { get; set; }

        /// <summary>
        ///     The email address
        /// </summary>
        /// <remarks>Before persisting the changes the email will be validated to make sure it has a valid format.</remarks>
        [Required, StringLength(CoreConstants.StringLength.TeamTunerUser.EMAIL)]
        public string Email { get; set; }

        /// <summary>
        ///     MD5 hash of the user password
        /// </summary>
        [Required, StringLength(AuthorizationConstants.StringLength.AuthorizationRequestDto.PASSWORD_MD5,
             MinimumLength = AuthorizationConstants.StringLength.AuthorizationRequestDto.PASSWORD_MD5)]
        public string PasswordMd5 { get; set; }
    }
}