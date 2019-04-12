using System;

namespace Sppd.TeamTuner.Core.Exceptions
{
    public class ArgumentException : BusinessException
    {
        public string ParameterName { get; }

        public ArgumentException(string parameterName)
        {
            ParameterName = parameterName;
        }

        public ArgumentException(string message, string parameterName) : base(message)
        {
            ParameterName = parameterName;
        }

        public ArgumentException(string message, string parameterName, Exception innerException) : base(message, innerException)
        {
            ParameterName = parameterName;
        }
    }
}