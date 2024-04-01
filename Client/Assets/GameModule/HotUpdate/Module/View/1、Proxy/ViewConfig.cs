using LitJson;
using VContainer;

namespace Ninth.HotUpdate
{
    public class ViewConfig : IViewConfig
    {
        public ViewAssetConfig ViewAssetConfig { get; }

        [Inject]
        public ViewConfig(PreLoadAssets preLoadAssets)
        {
            ViewAssetConfig = JsonMapper.ToObject<ViewAssetConfig>(preLoadAssets.ViewAssetConfig.text);
            ViewAssetConfig.Build();
        }
    }
}