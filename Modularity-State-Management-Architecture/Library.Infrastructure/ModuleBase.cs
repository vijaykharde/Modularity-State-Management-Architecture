using CsTech.Modularity;
using CsTech.Modularity.Configuration;
using StateBuilder.Library.Infrastructure;
using Unity;
namespace Library.Infrastructure
{
    public abstract class ModuleBase<TModuleConfigurationElement> : Module<TModuleConfigurationElement>
        where TModuleConfigurationElement : ModuleConfigurationElement
    {
        protected readonly IWorkItem RootWorkItem;

        protected ModuleBase(IWorkItem rootWorkItem, string moduleName)
            : base(moduleName)
        {
            RootWorkItem = rootWorkItem;
        }

        protected override void OnInitialize(TModuleConfigurationElement configurationElement)
        {
            RootWorkItem.Container.RegisterInstance(configurationElement);
        }
    }
}
