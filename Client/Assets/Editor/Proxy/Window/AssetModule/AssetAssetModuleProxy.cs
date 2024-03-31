using System.Linq;
using Ninth.HotUpdate;
using UnityEngine;
using VContainer;

namespace Ninth.Editor
{
    public class AssetAssetModuleProxy : IAssetModuleProxy 
    {
        private readonly IAssetModuleConfig assetModuleConfig;
        private readonly IObjectResolver resolver;
        
        [Inject]
        public AssetAssetModuleProxy(IAssetModuleConfig assetModuleConfig, IObjectResolver resolver)
        {
            this.assetModuleConfig = assetModuleConfig;
            this.resolver = resolver;
        }

        void IOnGUI.OnGUI()
        {
            Tab();
            Content();
        }
        
        void Tab()
        {
            var texts = assetModuleConfig.MappingSelector.Keys.ToArray().ToArrayString();
            var selected = assetModuleConfig.MappingSelector.CurrentIndex;
            using (new GUILayout.HorizontalScope())
            {
                EditorWindowUtility.Toolbar(selected, texts);
            }
        }

        void Content()
        {
            var type = assetModuleConfig.MappingSelector.CurrentValue;
            using (new GUILayout.VerticalScope())
            {
                (resolver.Resolve(type) as IOnGUI)?.OnGUI();
            }
        }
    }
}
