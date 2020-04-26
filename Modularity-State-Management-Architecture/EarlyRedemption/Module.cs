using CsTech.Modularity.Configuration;
using EarlyRedemption.BusinessObjects;
using EarlyRedemption.RequestProcessors;
using EarlyRedemption.WorkItems;
using Library.Infrastructure;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using StateBuilder.Library.Infrastructure;
using StateBuilder.RequestManager;
using Unity;

namespace EarlyRedemption
{
    [ConfigurationElementType(typeof(ModuleConfigurationElement))]
    public class Module : ModuleBase<ModuleConfigurationElement>
    {
        public Module(IWorkItem rootWorkItem)
            : base(rootWorkItem, "Module.EarlyRedemption")
        {
        }

        protected override void OnInitialize(ModuleConfigurationElement configurationElement)
        {
            base.OnInitialize(configurationElement);
            var requestProcessorRegistry = RootWorkItem.Container.Resolve<IRequestProcessorRegistry>();
            requestProcessorRegistry.RegisterProcessorType<ERQueryObject, ERResponseObject, ERSummaryRequestProcessor>();

            RootWorkItem.WorkItems.AddNew<EarlyRedemptionWorkItem>();
            IWorkItem workItem = RootWorkItem.WorkItems.Get<EarlyRedemptionWorkItem>();
            if (workItem.Status == WorkItemStatus.NotRunning)
            {
                workItem.Run();
            }
        }
    }
}
