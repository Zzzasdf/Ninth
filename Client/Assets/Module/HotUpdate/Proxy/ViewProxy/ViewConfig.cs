using Ninth.Utility;
using VContainer;

namespace Ninth.HotUpdate
{
    public class ViewConfig: IViewConfig
    {
        private readonly SubscriberCollect<string> stringSubscriber;
        private readonly SubscriberCollect<(string path, VIEW_HIERARCY hierarcy)> tupleSubscriber;
        SubscriberCollect<string> IViewConfig.StringSubscriber => stringSubscriber;
        SubscriberCollect<(string path, VIEW_HIERARCY hierarcy)> IViewConfig.TupleSubscriber => tupleSubscriber;
        
        [Inject]
        public ViewConfig()
        {
            {
                var build = stringSubscriber = new SubscriberCollect<string>();
                build.Subscribe<VIEW_HIERARCY>("Assets/GAssets/RemoteGroup/View/ViewLayout.prefab");
            }

            {
                var build = tupleSubscriber = new SubscriberCollect<(string path, VIEW_HIERARCY hierarcy)>();
                build.Subscribe<LoginView>(("Assets/GAssets/RemoteGroup/View/LoginView.prefab", VIEW_HIERARCY.Frame));
            }
        }
    }
}

