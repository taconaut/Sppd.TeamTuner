using System;

namespace Sppd.TeamTuner.Core.Exceptions
{
    /// <summary>
    ///     Base exception class for all application exceptions
    /// </summary>
    /// <seealso cref="System.Exception" />
    [Serializable]
    public class BusinessException : Exception
    {
        public BusinessException()
        {
        }

        public BusinessException(string message) : base(message)
        {
        }

        public BusinessException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}