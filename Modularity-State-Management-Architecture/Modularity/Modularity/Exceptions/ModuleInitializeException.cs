using System;
using System.Globalization;
using System.Runtime.Serialization;
using CsTech.Modularity.Constants;

namespace CsTech.Modularity.Exceptions
{
    /// <summary>
    /// Exception thrown by <see cref="IModuleInitializer"/> implementations whenever 
    /// a module fails to load.
    /// </summary>
    [Serializable]
    public class ModuleInitializeException : ModularityException
    {
        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ModuleInitializeException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleInitializeException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public ModuleInitializeException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleInitializeException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public ModuleInitializeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes the exception with a particular module and error message.
        /// </summary>
        /// <param name="moduleName">The name of the module.</param>
        /// <param name="moduleAssembly">The assembly where the module is located.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public ModuleInitializeException(string moduleName, string moduleAssembly, string message)
            : this(moduleName, message, moduleAssembly, null)
        {
        }

        /// <summary>
        /// Initializes the exception with a particular module, error message and inner exception 
        /// that happened.
        /// </summary>
        /// <param name="moduleName">The name of the module.</param>
        /// <param name="moduleAssembly">The assembly where the module is located.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, 
        /// or a <see langword="null"/> reference if no inner exception is specified.</param>
        public ModuleInitializeException(string moduleName, string moduleAssembly, string message, Exception innerException)
            : base(
                moduleName,
                String.Format(CultureInfo.CurrentCulture, ExceptionMessages.FailedToLoadModule, moduleName, moduleAssembly, message),
                innerException)
        {
        }

        /// <summary>
        /// Initializes the exception with a particular module, error message and inner exception that happened.
        /// </summary>
        /// <param name="moduleName">The name of the module.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, 
        /// or a <see langword="null"/> reference if no inner exception is specified.</param>
        public ModuleInitializeException(string moduleName, string message, Exception innerException)
            : base(
                moduleName,
                String.Format(CultureInfo.CurrentCulture, ExceptionMessages.FailedToLoadModuleNoAssemblyInfo, moduleName, message),
                innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected ModuleInitializeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}