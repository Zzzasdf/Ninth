using System.Linq;
using Ninth.HotUpdate;
using UnityEngine;
using VContainer;

namespace Ninth.Editor
{
    public class WindowProxy: IWindowProxy
    {
        private readonly IWindowConfig windowConfig;
        private readonly IObjectResolver resolver;
        
        [Inject]
        public WindowProxy(IWindowConfig windowConfig, IObjectResolver resolver)
        {
            this.windowConfig = windowConfig;
            this.resolver = resolver;
        }

        void IWindowProxy.Tab()
        {
            var texts = windowConfig.MappingSelector.Keys.ToArray().ToArrayString();
            var selected = windowConfig.MappingSelector.CurrentIndex;
            using (new GUILayout.VerticalScope())
            {
                EditorWindowUtility.SelectionGrid(selected, texts, 1);
            }
        }

        void IWindowProxy.Content()
        {
            var type = windowConfig.MappingSelector.CurrentValue;
            using (new GUILayout.VerticalScope())
            {
                (resolver.Resolve(type) as IOnGUI)?.OnGUI();
            }
        }
    }
}