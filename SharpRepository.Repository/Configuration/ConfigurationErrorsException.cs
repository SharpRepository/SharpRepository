using System;

namespace SharpRepository.Repository.Configuration
{
#if NETSTANDARD1_6
    public class ConfigurationErrorsException : Exception
    {
        public ConfigurationErrorsException()
            : base()
        { }

        public ConfigurationErrorsException(string message)
            : base(message)
        { }

        public ConfigurationErrorsException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
#endif
}
