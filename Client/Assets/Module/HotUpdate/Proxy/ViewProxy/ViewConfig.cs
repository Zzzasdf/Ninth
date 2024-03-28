using Ninth.Utility;
using VContainer;

namespace Ninth.HotUpdate
{
    public class ViewConfig: IViewConfig
    {
        private readonly SubscriberCollect<string> stringSubscriber;
        private readonly SubscriberCollect<ViewInfo> viewInfoSubscriber;
        SubscriberCollect<string> IViewConfig.StringSubscriber => stringSubscriber;
        SubscriberCollect<ViewInfo> IViewConfig.ViewInfoSubscriber => viewInfoSubscriber;
        
        [Inject]
        public ViewConfig()
        {
            {
                var build = stringSubscriber = new SubscriberCollect<string>();
                build.Subscribe<VIEW_HIERARCHY>("Assets/GAssets/RemoteGroup/Views/ViewLayout.prefab");
            }

            {
                viewInfoSubscriber = new SubscriberCollect<ViewInfo>();
                ViewSubscribe<LoginView>("Assets/GAssets/RemoteGroup/Views/Login/LoginView.prefab", VIEW_HIERARCHY.Frame);
                ViewSubscribe<SettingsView>("Assets/GAssets/RemoteGroup/Views/Settings/SettingsView.prefab", VIEW_HIERARCHY.Frame);
            }
        }
        
        private void ViewSubscribe<T>(string path, VIEW_HIERARCHY hierarchy) where T: BaseView
            => ViewSubscribe<T>(path, hierarchy, 10);

        private void ViewSubscribe<T>(string path, VIEW_HIERARCHY hierarchy, int weights) where T: BaseView
            => viewInfoSubscriber.Subscribe<T>(new ViewInfo(path, hierarchy, weights));

        public class ViewInfo
        {
            public readonly string Path;
            public readonly VIEW_HIERARCHY Hierarchy;
            public readonly int Weight;

            public ViewInfo(string path, VIEW_HIERARCHY hierarchy, int weight)
            {
                this.Path = path;
                this.Hierarchy = hierarchy;
                this.Weight = weight;
            }

            public void Deconstruct(out string path, out VIEW_HIERARCHY hierarchy, out int weight)
            {
                path = this.Path;
                hierarchy = this.Hierarchy;
                weight = this.Weight;
            }
        }
    }
}

