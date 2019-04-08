using System;

namespace Sppd.TeamTuner.Core.Exceptions
{
    /// <summary>
    ///     Thrown when a security issue has been encountered.
    /// </summary>
    /// <seealso cref="BusinessException" />
    [Serializable]
    public class SecurityException : BusinessException
    {
        public SecurityException()
        {
        }

        public SecurityException(string message) : base(message)
        {
        }

        public SecurityException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}