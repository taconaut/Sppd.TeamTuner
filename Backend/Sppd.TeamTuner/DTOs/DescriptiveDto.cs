using System.ComponentModel.DataAnnotations;

using Sppd.TeamTuner.Core;

namespace Sppd.TeamTuner.DTOs
{
    public class DescriptiveDto : NamedDto
    {
        /// <summary>
        ///     The avatar
        /// </summary>
        public byte[] Avatar { get; set; }

        /// <summary>
        ///     The description
        /// </summary>
        [StringLength(CoreConstants.StringLength.Descriptive.DESCRIPTION)]
        public string Description { get; set; }
    }
}