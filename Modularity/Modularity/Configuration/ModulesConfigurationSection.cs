using System;
using System.Configuration;

namespace CsTech.Modularity.Configuration
{
    /// <summary>
    /// A <see cref="ConfigurationSection"/> for module configuration.
    /// </summary>
    public class ModulesConfigurationSection : ConfigurationSection
    {
        /// <summary>
        /// Gets or sets the collection of modules configuration.
        /// </summary>
        /// <value>A <seealso cref="ModuleConfigurationElementCollection"/> of <seealso cref="ModuleConfigurationElement"/>.</value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [ConfigurationProperty("", IsDefaultCollection = true, IsKey = false)]
        [ConfigurationCollection(typeof(ModuleConfigurationElementCollection), AddItemName = "module")]
        public ModuleConfigurationElementCollection Modules
        {
            get { return (ModuleConfigurationElementCollection)base[string.Empty]; }
            set { base[string.Empty] = value; }
        }
    }
}