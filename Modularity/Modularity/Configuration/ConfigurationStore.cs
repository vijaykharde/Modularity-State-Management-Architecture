using System;
using System.Configuration;

namespace CsTech.Modularity.Configuration
{
    /// <summary>
    /// Defines a store for the module metadata.
    /// </summary>
    public class ConfigurationStore
    {
        /// <summary>
        /// Gets the module configuration data.
        /// </summary>
        /// <returns>A <see cref="ModulesConfigurationSection"/> instance.</returns>
        public ModulesConfigurationSection RetrieveModuleConfigurationSection()
        {
            return ConfigurationManager.GetSection("modules") as ModulesConfigurationSection;
        }
    }
}