using System.ComponentModel.DataAnnotations;

namespace Sppd.TeamTuner.Core.Domain.Entities
{
    /// <summary>
    ///     Holds a description an avatar.
    /// </summary>
    /// <seealso cref="NamedEntity" />
    public abstract class DescriptiveEntity : NamedEntity
    {
        public byte[] Avatar { get; set; }

        [StringLength(CoreConstants.StringLength.Descriptive.DESCRIPTION)]
        public string Description { get; set; }
    }
}