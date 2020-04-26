using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System;
using System.Configuration;

namespace CsTech.Modularity.Configuration
{
    /// <summary>
    /// A configuration element to declare module metadata.
    /// </summary>
    public class ModuleConfigurationElement : NameTypeConfigurationElement
    {
        public ModuleConfigurationElement()
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="moduleName"></param>
        /// <param name="moduleType"></param>
        public ModuleConfigurationElement(string moduleName, Type moduleType)
            : base(moduleName, moduleType)
        {
        }

        /// <summary>
        /// Gets or sets the assembly file.
        /// </summary>
        /// <value>The assembly file.</value>
        [ConfigurationProperty("assemblyFile", IsRequired = true)]
        public string AssemblyFile
        {
            get => (string)base["assemblyFile"];
            set => base["assemblyFile"] = value;
        }

        /// <summary>
        /// Gets or sets the modules this module depends on.
        /// </summary>
        /// <value>The names of the modules that this depends on.</value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [ConfigurationProperty("dependencies", IsDefaultCollection = true, IsKey = false)]
        public ModuleDependencyConfigurationElementCollection Dependencies
        {
            get => (ModuleDependencyConfigurationElementCollection)base["dependencies"];
            set => base["dependencies"] = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the module should be loaded at startup.
        /// </summary>
        /// <value>A value indicating whether the module should be loaded at startup.</value>
        [ConfigurationProperty("startupLoaded", IsRequired = false, DefaultValue = true)]
        public bool StartupLoaded
        {
            get => (bool)base["startupLoaded"];
            set => base["startupLoaded"] = value;
        }
    }
}