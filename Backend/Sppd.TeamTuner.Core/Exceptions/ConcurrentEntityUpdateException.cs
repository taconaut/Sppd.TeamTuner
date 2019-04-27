using System;

namespace Sppd.TeamTuner.Core.Exceptions
{
    public class ConcurrentEntityUpdateException : BusinessException
    {
        public ConcurrentEntityUpdateException()
        {
        }

        public ConcurrentEntityUpdateException(string message) : base(message)
        {
        }

        public ConcurrentEntityUpdateException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}