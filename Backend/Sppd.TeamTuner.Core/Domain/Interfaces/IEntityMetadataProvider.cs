namespace Sppd.TeamTuner.Core.Domain.Interfaces
{
    /// <summary>
    ///     Implement this interface to set entity metadata before it is being persisted.
    /// </summary>
    public interface IEntityMetadataProvider
    {
        /// <summary>
        ///     Set modifier metadata on entity when this method is being called.
        /// </summary>
        void SetModifierMetadataOnChangedEntities();
    }
}