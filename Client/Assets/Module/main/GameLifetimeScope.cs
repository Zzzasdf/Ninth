using Cysharp.Threading.Tasks;
using Ninth.Utility;
using UnityEngine;
using UnityEngine.Networking;
using VContainer;
using VContainer.Unity;

namespace Ninth
{
    public class GameLifetimeScope : LifetimeScope
    {
#if UNITY_EDITOR
        protected override void Configure(IContainerBuilder builder)
        {
            "Main 初始化".FrameLog();
            builder.Register<LoadCSharp>(Lifetime.Scoped);
            builder.UseEntryPoints(Lifetime.Singleton, entryPoints =>
            {
                entryPoints.Add<LoadCSharp>();
                entryPoints.OnException(ex => ex.FrameError());
            });
            "Main IOC 容器注册完成！！".FrameLog();
        }
#else
        protected override async void Configure(IContainerBuilder builder)
        {
            "Main 初始化".FrameLog();
            var nameConfig = Resources.Load<NameConfig>("SOData/NameConfigSO");
            var request = UnityWebRequest.Get($"{Application.streamingAssetsPath}/{(nameConfig as INameConfig).FileNameByVersionConfig()}");
            await request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                request.error.Error();
                return;
            }
            var playerVersionConfig = LitJson.JsonMapper.ToObject<PlayerVersionConfig>(request.downloadHandler.text);
            builder.RegisterInstance(playerVersionConfig).AsSelf();
            builder.RegisterInstance(nameConfig).As<INameConfig>();
            builder.Register<LoadDll>(Lifetime.Scoped);

            switch (playerVersionConfig.Env)
            {
                case Environment.Local:
                {
                    builder.UseEntryPoints(Lifetime.Singleton, entryPoints =>
                    {
                        entryPoints.Add<LoadDll>();
                        entryPoints.OnException(ex => ex.FrameError());
                    });
                    break;
                }
                case Environment.Remote:
                {
                    builder.Register<PlayerSettingsConfig>(Lifetime.Singleton).As<IPlayerSettingsConfig>();
                    builder.Register<VersionPathConfig>(Lifetime.Singleton).As<IVersionPathConfig>();
                    builder.Register<ConfigPathConfig>(Lifetime.Singleton).As<IConfigPathConfig>(); 
                    builder.Register<BundlePathConfig>(Lifetime.Singleton).As<IBundlePathConfig>();
                    builder.Register<PathProxy>(Lifetime.Singleton).As<IPathProxy>();
                    builder.Register<JsonConfig>(Lifetime.Singleton).As<IJsonConfig>();
                    builder.Register<JsonProxy>(Lifetime.Singleton).As<IJsonProxy>();
                    builder.Register<PlayerPrefsConfig>(Lifetime.Singleton).As<IPlayerPrefsIntConfig, IPlayerPrefsFloatConfig, IPlayerPrefsStringConfig>();
                    builder.Register<PlayerPrefsProxy>(Lifetime.Singleton).As<IPlayerPrefsIntProxy, IPlayerPrefsFloatProxy, IPlayerPrefsStringProxy>();
                    builder.Register<DownloadProxy>(Lifetime.Singleton).As<IDownloadProxy>();
                    builder.Register<AssetDownloadBox>(Lifetime.Scoped).As<IAssetDownloadBox>();
                    builder.Register<AssetBundleProxy>(Lifetime.Scoped).As<IAssetBundleProxy>();
                    builder.UseEntryPoints(Lifetime.Singleton, entryPoints =>
                    {
                        entryPoints.Add<AssetBundleProxy>();
                        entryPoints.OnException(ex => ex.FrameError());
                    });
                    break;
                }
            }
            "Main IOC 容器注册完成！！".FrameLog(); 
        }
#endif
    }
}