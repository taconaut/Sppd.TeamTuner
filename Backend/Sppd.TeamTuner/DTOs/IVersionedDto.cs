namespace Sppd.TeamTuner.DTOs
{
    internal interface IVersionedDto
    {
        /// <summary>
        ///     The entity version.
        /// </summary>
        /// <remarks>
        ///     Used for optimistic locking. If the specified version is different then the one stored in database, the update
        ///     will be rejected.
        /// </remarks>
        string Version { get; set; }
    }
}