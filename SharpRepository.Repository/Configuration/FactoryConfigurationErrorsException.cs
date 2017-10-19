using System;

namespace SharpRepository.Repository.Configuration
{
    public class FactoryConfigurationErrorsException : ConfigurationErrorsException
    {
        const string MessageTemplate = "Configuration error: Factory class \"{0}\" not found";

        public FactoryConfigurationErrorsException()
            : base()
        { }

        public FactoryConfigurationErrorsException(string factoryClassName)
            : base(string.Format(MessageTemplate, factoryClassName))
        { }

        public FactoryConfigurationErrorsException(string factoryClassName, Exception innerException)
            : base(string.Format(MessageTemplate, factoryClassName), innerException)
        { }
    }
}
