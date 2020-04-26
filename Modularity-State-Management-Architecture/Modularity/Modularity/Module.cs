using System;
using System.Configuration;
using CsTech.Modularity.Configuration;

namespace CsTech.Modularity
{
    public abstract class Module<TModuleConfigurationElement> : IModule
        where TModuleConfigurationElement : ModuleConfigurationElement
    {
        protected Module(string moduleName)
        {
            ModuleName = moduleName;
        }

        public readonly string ModuleName;

        public void Initialize(ConfigurationElement configurationElement)
        {
            var moduleConfigurationElement = configurationElement as TModuleConfigurationElement;
            if (moduleConfigurationElement == null)
            {
                throw new ArgumentException($"Unable to cast type of {configurationElement.GetType().FullName} to {typeof (TModuleConfigurationElement).FullName}", nameof(configurationElement));
            }
            OnInitialize(moduleConfigurationElement);
        }

        protected virtual void OnInitialize(TModuleConfigurationElement configurationElement)
        {
        }
    }
}
