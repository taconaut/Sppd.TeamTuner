using System.Collections.Generic;

using Sppd.TeamTuner.Core.Domain.Validation;
using Sppd.TeamTuner.Core.Validation;

namespace Sppd.TeamTuner.Core.Domain.Entities
{
    /// <summary>
    ///     Specifies a rarity (e.g. Common, Legendary)
    /// </summary>
    /// <seealso cref="CoreData" />
    public class Rarity : CoreData
    {
        public int FriendlyLevel { get; set; }

        public override IEnumerable<EntityValidationError> Validate(IValidationContext context)
        {
            if (FriendlyLevel < 1 || FriendlyLevel > 7)
            {
                var msg = $"Must be between 1-7. Current value is {FriendlyLevel}";
                yield return new EntityValidationError(msg, nameof(FriendlyLevel));
            }
        }
    }
}