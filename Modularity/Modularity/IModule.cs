using System;
using System.Configuration;

namespace CsTech.Modularity
{
    /// <summary>
    /// Defines the contract for the modules deployed in the application.
    /// </summary>
    public interface IModule
    {
        /// <summary>
        /// Notifies the module that it has be initialized.
        /// </summary>
        /// <param name="configurationElement">The <see cref="ConfigurationElement"/> for the module.</param>
        void Initialize(ConfigurationElement configurationElement);
    }
}