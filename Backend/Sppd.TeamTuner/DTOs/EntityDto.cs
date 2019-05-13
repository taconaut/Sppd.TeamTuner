using System;
using System.ComponentModel.DataAnnotations;

namespace Sppd.TeamTuner.DTOs
{
    public abstract class EntityDto : IVersionedDto
    {
        /// <summary>
        ///     The entity identifier
        /// </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        ///     The UTC time, when the entity has been created
        /// </summary>
        [Required]
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        ///     The UTC time, when the entity has been last updated
        /// </summary>
        [Required]
        public DateTime ModifiedOnUtc { get; set; }

        /// <summary>
        ///     The entity version.
        /// </summary>
        /// <remarks>
        ///     Used for optimistic locking. If the specified version is different then the one stored in database, the update
        ///     will be rejected.
        /// </remarks>
        [Required]
        public string Version { get; set; }
    }
}