using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace CsTech.Modularity.Exceptions
{
    /// <summary>
    /// Base class for exceptions that are thrown because of a problem with modules. 
    /// </summary>
    [Serializable]
    public class ModularityException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModularityException"/> class.
        /// </summary>
        public ModularityException()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModularityException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public ModularityException(string message)
            : this(null, message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModularityException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public ModularityException(string message, Exception innerException)
            : this(null, message, innerException)
        {
        }

        /// <summary>
        /// Initializes the exception with a particular module and error message.
        /// </summary>
        /// <param name="moduleName">The name of the module.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public ModularityException(string moduleName, string message)
            : this(moduleName, message, null)
        {
        }

        /// <summary>
        /// Initializes the exception with a particular module, error message and inner exception that happened.
        /// </summary>
        /// <param name="moduleName">The name of the module.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, 
        /// or a <see langword="null"/> reference if no inner exception is specified.</param>
        public ModularityException(string moduleName, string message, Exception innerException)
            : base(message, innerException)
        {
            ModuleName = moduleName;
        }

        /// <summary>
        /// Initializes a new instance with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected ModularityException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            ModuleName = info.GetValue("ModuleName", typeof(string)) as string;
        }

        /// <summary>
        /// Sets the <see cref="System.Runtime.Serialization.SerializationInfo"/> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("ModuleName", ModuleName);
        }

        /// <summary>
        /// Gets or sets the name of the module that this exception refers to.
        /// </summary>
        /// <value>The name of the module.</value>
        public string ModuleName { get; set; }
    }
}
