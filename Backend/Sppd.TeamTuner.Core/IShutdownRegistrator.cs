namespace Sppd.TeamTuner.Core
{
    public interface IShutdownRegistrator
    {
        /// <summary>
        ///     Called before the application shuts down.
        /// </summary>
        void OnBeforeShutdown();
    }
}