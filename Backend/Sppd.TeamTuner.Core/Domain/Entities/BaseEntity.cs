using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Sppd.TeamTuner.Core.Domain.Interfaces;
using Sppd.TeamTuner.Core.Domain.Validation;
using Sppd.TeamTuner.Core.Validation;

namespace Sppd.TeamTuner.Core.Domain.Entities
{
    /// <summary>
    ///     All entities have to extend this base class.
    /// </summary>
    /// <seealso cref="IValidatable" />
    public abstract class BaseEntity : IValidatable
    {
        /// <summary>
        ///     Entity unique identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        ///     Specifies when the entity instance has been created.
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        ///     Specifies by whom the entity instance has been created.
        /// </summary>
        public Guid CreatedById { get; set; }

        /// <summary>
        ///     Specifies when the entity instance has been last updated.
        /// </summary>
        public DateTime ModifiedOnUtc { get; set; }

        /// <summary>
        ///     Specifies by whom the entity instance has been last modified.
        /// </summary>
        public Guid ModifiedById { get; set; }

        /// <summary>
        ///     Flag indicating if an entity has been soft deleted.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        ///     Specifies when the entity instance has been deleted.
        /// </summary>
        public DateTime? DeletedOnUtc { get; set; }

        /// <summary>
        ///     Specifies by whom the entity instance has been deleted.
        /// </summary>
        public Guid? DeletedById { get; set; }

        /// <summary>
        ///     Gets or sets the version of the entity.
        ///     Used for optimistic locking.
        /// </summary>
        [Timestamp]
        public byte[] Version { get; set; }

        protected BaseEntity()
        {
            Id = Guid.NewGuid();
        }

        public virtual IEnumerable<EntityValidationError> Validate(IValidationContext context)
        {
            yield break;
        }
    }
}