using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsTech.Modularity.Constants;
using CsTech.Modularity.Extensions;

namespace CsTech.Modularity.Configuration
{
    /// <summary>
    /// A catalog built from a configuration file.
    /// </summary>
    public class ConfigurationModuleCatalog : ModuleCatalog
    {
        /// <summary>
        /// Builds an instance of ConfigurationModuleCatalog with a <see cref="ConfigurationStore"/> as the default store.
        /// </summary>
        public ConfigurationModuleCatalog()
        {
            Store = new ConfigurationStore();
        }
        ModulesConfigurationSection _modulesConfigurationSection;
        public ConfigurationModuleCatalog(ModulesConfigurationSection modulesConfigurationSection)
        {
            _modulesConfigurationSection = modulesConfigurationSection;
            Store = new ConfigurationStore();
        }

        /// <summary>
        /// Gets or sets the store where the configuration is kept.
        /// </summary>
        public ConfigurationStore Store { get; set; }

        /// <summary>
        /// Loads the catalog from the configuration.
        /// </summary>
        protected override void InnerLoad()
        {
            if (Store == null)
            {
                throw new InvalidOperationException(ExceptionMessages.ConfigurationStoreCannotBeNull);
            }

            EnsureModulesDiscovered();
        }

        private static string GetFileAbsoluteUri(string filePath)
        {
            var uriBuilder = new UriBuilder
            {
                Host = string.Empty,
                Scheme = Uri.UriSchemeFile,
                Path = Path.GetFullPath(filePath)
            };
            var fileUri = uriBuilder.Uri;
            return fileUri.ToString();
        }

        private void EnsureModulesDiscovered()
        {
            //ModulesConfigurationSection configurationSection = new ModulesConfigurationSection();

            //var modules = new ModuleConfigurationElementCollection(new ModuleConfigurationElement[] { new ModuleConfigurationElement() { Name = "Module.CommunityWatch", AssemblyFile = "Module.CommunityWatch.dll", TypeName = "Module.CommunityWatch.Module, Module.CommunityWatch" } });
            //configurationSection.Modules = modules;
            var section = _modulesConfigurationSection;// configurationSection;// Store.RetrieveModuleConfigurationSection();

            if (section != null)
            {
                foreach (var element in section.Modules)
                {
                    IList<string> dependencies = new List<string>();

                    if (element.Dependencies.Count > 0)
                    {
                        foreach (ModuleDependencyConfigurationElement dependency in element.Dependencies)
                        {
                            dependencies.Add(dependency.ModuleName);
                        }
                    }

                    var moduleInfo = new ModuleInfo(element.Name, element.TypeName)
                    {
                        Ref = GetFileAbsoluteUri(element.AssemblyFile),
                        InitializationMode = element.StartupLoaded ? InitializationMode.WhenAvailable : InitializationMode.OnDemand,
                        ConfigurationElement = element
                    };
                    moduleInfo.DependsOn.AddRange(dependencies.ToArray());
                    AddModule(moduleInfo);
                }
            }
        }
    }
}
