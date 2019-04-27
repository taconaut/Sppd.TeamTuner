using System;

namespace Sppd.TeamTuner.Core.Exceptions
{
    public class EntityUpdateException : BusinessException
    {
        public EntityUpdateException()
        {
        }

        public EntityUpdateException(string message) : base(message)
        {
        }

        public EntityUpdateException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}