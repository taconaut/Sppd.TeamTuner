using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sppd.TeamTuner.DTOs
{
    public class UpdateRequestDto : IVersionedDto
    {
        /// <summary>
        ///     The identifier of the entity to update
        /// </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        ///     Only the properties specified in this list will be updated. If NULL or empty, all properties will be updated.
        /// </summary>
        public ISet<string> PropertiesToUpdate { get; set; }

        /// <summary>
        ///     The entity version
        /// </summary>
        /// <remarks>
        ///     Used for optimistic locking. If the specified version is different then the one stored in database, the update
        ///     will be rejected.
        /// </remarks>
        [Required]
        public string Version { get; set; }
    }
}