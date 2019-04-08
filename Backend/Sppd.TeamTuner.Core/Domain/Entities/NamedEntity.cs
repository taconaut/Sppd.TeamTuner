using System.ComponentModel.DataAnnotations;

namespace Sppd.TeamTuner.Core.Domain.Entities
{
    /// <summary>
    ///     Holds a name
    /// </summary>
    /// <seealso cref="BaseEntity" />
    public abstract class NamedEntity : BaseEntity
    {
        [Required, StringLength(CoreConstants.StringLength.Named.NAME)]
        public string Name { get; set; }
    }
}