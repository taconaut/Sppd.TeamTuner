using System.ComponentModel.DataAnnotations;

namespace Sppd.TeamTuner.DTOs
{
    public class UserAuthorizationResponseDto : UserResponseDto
    {
        /// <summary>
        ///     The token which will have to be set as bearer in the HTTP authorization header for subsequent calls requiring
        ///     authentication
        /// </summary>
        [Required]
        public string Token { get; set; }
    }
}