using Microsoft.Practices.CompositeUI;
using Unity;

namespace WebApplicationNG
{
    public class ShellWorkItem : WorkItem
    {
        public IUnityContainer Container { get; } = new UnityContainer();

        protected override void InitializeServices()
        {
            base.InitializeServices();
            Items.Add(Container, "PublicContainer");
            /*ICommandAdapterMapService mapService = RootWorkItem.Services.Get<ICommandAdapterMapService>();
            mapService.Register(typeof(FrameworkElement), typeof(WPFCommandCommandAdapter));
            mapService.Register(typeof(UIElement), typeof(UIElementCommandAdapter));

            IUIElementAdapterFactoryCatalog uiElementAdapterFactoryCatalog = RootWorkItem.Services.Get<IUIElementAdapterFactoryCatalog>();
            uiElementAdapterFactoryCatalog.RegisterFactory(new ItemsControlUIAdapterFactory());

            RootWorkItem.Services.AddNew<CacheService, ICacheService>();

            Items.Add(Container, ItemNames.PublicContainer);*/
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            /*var config = Services.Get<IConfigurationService>();
            var task = config.GetUserSettingAsync(ConstantNames.KeyGlobalZoom);
            task.ContinueWith((t) =>
            {
                var setting = t.Result;
                var zoom = setting.ToDoubleSafe();
                if (zoom.HasValue) AppPreferences.All.Zoom = zoom.Value;
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
            var task2 = config.GetUserSettingAsync(ConstantNames.KeyPreferredLanguage);
            task2.ContinueWith((t) =>
            {
                var setting = t.Result;
                if (!string.IsNullOrEmpty(setting))
                {
                    AppPreferences.All.Language = setting;
                    var workItem = Container.Resolve<IWorkItem>();
                    workItem.State[StateNames.PreferredLocale] = setting;
                }
                else
                {
                    AppPreferences.All.Language = "en-US";
                    var workItem = Container.Resolve<IWorkItem>();
                    workItem.State[StateNames.PreferredLocale] = "en-US";
                }
            }, TaskContinuationOptions.OnlyOnRanToCompletion);*/
        }
    }
}
