using Library.Infrastructure.Library.Infrastructure.Services;
using Library.Infrastructure.Library.Interface.Services;
using Library.Infrastructure.Library.StateBuilders;
using Library.Infrastructure.Web.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using StateBuilder.Library.Infrastructure;
using StateBuilder.Library.Interface.Constants;
using StateBuilder.RequestManager;
using StateBuilder.RequestManager.Unity;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity;
using Unity.Lifetime;
namespace WebApplicationNG
{
    public class ShellApplication
    {
        public int RootWorkItem_AllModulesLoaded { get; private set; }
        IWorkItem WorkItem { get; set; }

        protected void Start()
        {
            WebHost.CreateDefaultBuilder().Configure((app)=> {
                app.UseSignalR(routes =>
                {
                    routes.MapHub<NotifyHub>("/notify");
                });
            }).ConfigureServices((context,services) =>
            {
                services.AddSingleton(typeof(IWorkItem), WorkItem);
            }).UseStartup<Startup>().Build().Run();
        }

        private WebUnityBootstrapper _bootstrapper;
        public void Run()
        {
            OnRootWorkItemInitialized();
            WorkItem.Run();
            Start();

            WorkItem.Dispose();
        }
        protected void OnRootWorkItemInitialized()
        {
            var workItem = new __WorkItem(new UnityContainer());
            workItem.Container.RegisterInstance<IStateRegistry>(new StateRegistry());
            workItem.Container.RegisterInstance<IRequestProcessorRegistry>(new RequestProcessorRegistry());
            workItem.Container.RegisterType<IRequestProcessorResolver, RequestProcessorResolver>(new HierarchicalLifetimeManager());
            workItem.Container.RegisterType<IRequestManagerService, RequestManagerService>();
            workItem.Container.RegisterType<IRequestObserver, RequestObserver>();
            workItem.Container.RegisterType<IRequestProcessorResolverOverridesRegistry, RequestProcessorResolverOverridesRegistry>();
            workItem.Container.RegisterInstance<IEntityTranslatorService>(new EntityTranslatorService());
            workItem.Container.RegisterInstance<IHttpContextAccessor>(new HttpContextAccessor());
            workItem.Container.RegisterInstance<IList<Task>>(new List<Task>());
            workItem.Container.RegisterInstance<IList<Task<RequestContext<object, object>>>>(new List<Task<RequestContext<object, object>>>());

            workItem.State[key: StateNames.IsAuthenticated] = false;

            var _entityTranslatorService = workItem.Container.Resolve<IEntityTranslatorService>();
            _entityTranslatorService.RegisterEntityTranslator(new JSONToEntityTranslator(workItem));

            _bootstrapper = new WebUnityBootstrapper(workItem.Container);
            _bootstrapper.Run(true);
            WorkItem = workItem;
        }

    }
    public class __WorkItem : StateBuilder.Library.Infrastructure.WorkItem
    {
        public __WorkItem(IUnityContainer container)
            : base(container)
        {
        }
    }
}
