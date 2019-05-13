using System.ComponentModel.DataAnnotations;

using Sppd.TeamTuner.Authorization;
using Sppd.TeamTuner.Core;

namespace Sppd.TeamTuner.DTOs
{
    public class AuthorizationRequestDto
    {
        /// <summary>
        ///     User name
        /// </summary>
        [Required, StringLength(CoreConstants.StringLength.Named.NAME_MAX, MinimumLength = CoreConstants.StringLength.Named.NAME_MIN)]
        public string Name { get; set; }

        /// <summary>
        ///     MD5 hash of the user password
        /// </summary>
        [Required, StringLength(AuthorizationConstants.StringLength.AuthorizationRequestDto.PASSWORD_MD5,
             MinimumLength = AuthorizationConstants.StringLength.AuthorizationRequestDto.PASSWORD_MD5)]
        public string PasswordMd5 { get; set; }
    }
}