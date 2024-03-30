using System.Linq;
using Ninth.HotUpdate;
using UnityEngine;
using VContainer;

namespace Ninth.Editor
{
    public class ModuleProxy : IModuleProxy 
    {
        private readonly IModuleConfig moduleConfig;
        private readonly IObjectResolver resolver;
        
        [Inject]
        public ModuleProxy(IModuleConfig moduleConfig, IObjectResolver resolver)
        {
            this.moduleConfig = moduleConfig;
            this.resolver = resolver;
        }

        void IOnGUI.OnGUI()
        {
            Tab();
            Content();
        }
        
        void Tab()
        {
            var texts = moduleConfig.MappingSelector.Keys.ToArray().ToArrayString();
            var selected = moduleConfig.MappingSelector.CurrentIndex;
            using (new GUILayout.VerticalScope())
            {
                EditorWindowUtility.SelectionGrid(selected, texts, 1);
            }
        }

        void Content()
        {
            var type = moduleConfig.MappingSelector.CurrentValue;
            using (new GUILayout.VerticalScope())
            {
                (resolver.Resolve(type) as IOnGUI)?.OnGUI();
            }
        }
    }
}
