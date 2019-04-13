using System;

namespace Sppd.TeamTuner.Core.Exceptions
{
    public class ConcurrentUpdateException : BusinessException
    {
        public ConcurrentUpdateException()
        {
        }

        public ConcurrentUpdateException(string message) : base(message)
        {
        }

        public ConcurrentUpdateException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}