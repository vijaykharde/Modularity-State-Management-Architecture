using CsTech.Modularity.Constants;
using CsTech.Modularity.Exceptions;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace CsTech.Modularity.Unity
{
    /// <summary>
    /// Implements the <see cref="IModuleInitializer"/> interface. Handles loading of a module based on a type.
    /// </summary>
    internal class ModuleInitializer : IModuleInitializer
    {
        private readonly IServiceLocator _serviceLocator;

        /// <summary>
        /// Initializes a new instance of <see cref="ModuleInitializer"/>.
        /// </summary>
        /// <param name="serviceLocator">The container that will be used to resolve the modules by specifying its type.</param>
        public ModuleInitializer(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator ?? throw new ArgumentNullException(nameof(serviceLocator));
        }

        /// <summary>
        /// Initializes the specified module.
        /// </summary>
        /// <param name="moduleInfo">The module to initialize</param>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Catches Exception to handle any exception thrown during the initialization process with the HandleModuleInitializationError method.")]
        public void Initialize(ModuleInfo moduleInfo)
        {
            if (moduleInfo == null)
            {
                throw new ArgumentNullException(nameof(moduleInfo));
            }

            IModule moduleInstance = null;
            try
            {
                moduleInstance = CreateModule(moduleInfo);
                moduleInstance.Initialize(moduleInfo.ConfigurationElement);
            }
            catch (Exception ex)
            {
                HandleModuleInitializationError(moduleInfo, moduleInstance?.GetType().Assembly.FullName, ex);
            }
        }

        /// <summary>
        /// Handles any exception occurred in the module Initialization process and throws a <seealso cref="ModuleInitializeException"/>.
        /// This method can be overridden to provide a different behavior. 
        /// </summary>
        /// <param name="moduleInfo">The module metadata where the error happened.</param>
        /// <param name="assemblyName">The assembly name.</param>
        /// <param name="exception">The exception thrown that is the cause of the current error.</param>
        /// <exception cref="ModuleInitializeException"></exception>
        public virtual void HandleModuleInitializationError(ModuleInfo moduleInfo, string assemblyName, Exception exception)
        {
            if (moduleInfo == null)
            {
                throw new ArgumentNullException(nameof(moduleInfo));
            }
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            Exception moduleException;

            if (exception is ModuleInitializeException)
            {
                moduleException = exception;
            }
            else
            {
                moduleException = !string.IsNullOrEmpty(assemblyName) ? new ModuleInitializeException(moduleInfo.ModuleName, assemblyName, exception.Message, exception)
                                                                      : new ModuleInitializeException(moduleInfo.ModuleName, exception.Message, exception);
            }

            throw moduleException;
        }

        /// <summary>
        /// Uses the container to resolve a new <see cref="IModule"/> by specifying its <see cref="Type"/>.
        /// </summary>
        /// <param name="moduleInfo">The module to create.</param>
        /// <returns>A new instance of the module specified by <paramref name="moduleInfo"/>.</returns>
        protected virtual IModule CreateModule(ModuleInfo moduleInfo)
        {
            if (moduleInfo == null)
            {
                throw new ArgumentNullException(nameof(moduleInfo));
            }
            return CreateModule(moduleInfo.ModuleType);
        }

        /// <summary>
        /// Uses the container to resolve a new <see cref="IModule"/> by specifying its <see cref="Type"/>.
        /// </summary>
        /// <param name="typeName">The type name to resolve. This type must implement <see cref="IModule"/>.</param>
        /// <returns>A new instance of <paramref name="typeName"/>.</returns>
        protected virtual IModule CreateModule(string typeName)
        {
            Type moduleType = Type.GetType(typeName);
            if (moduleType == null)
            {
                throw new ModuleInitializeException(string.Format(CultureInfo.CurrentCulture, ExceptionMessages.FailedToGetType, typeName));
            }

            return (IModule)_serviceLocator.GetInstance(moduleType);
        }
    }
}
