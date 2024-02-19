using Ninth.Utility;
using UnityEditor;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Ninth.Editor
{
    [InitializeOnLoad]  
    public class EditorLifetimeScope
    {
        static EditorLifetimeScope()
        {
            var builder = new ContainerBuilder();
            
            // core
            var assetConfig = Resources.Load<AssetConfig>("SOData/AssetConfigSO");
            var nameConfig = Resources.Load<NameConfig>("SOData/NameConfigSO");
            builder.RegisterInstance(assetConfig).As<IAssetConfig>();
            builder.RegisterInstance(nameConfig).As<INameConfig>();
            
            builder.Register<PlayerSettingsConfig>(Lifetime.Singleton).As<IPlayerSettingsConfig>();
            builder.Register<VersionPathConfig>(Lifetime.Singleton).As<IVersionPathConfig>();
            builder.Register<ConfigPathConfig>(Lifetime.Singleton).As<IConfigPathConfig>(); 
            builder.Register<BundlePathConfig>(Lifetime.Singleton).As<IBundlePathConfig>();
            builder.Register<PathProxy>(Lifetime.Singleton).As<IPathProxy>();
            
            builder.Register<JsonConfig>(Lifetime.Singleton).As<IJsonConfig>();
            builder.Register<JsonProxy>(Lifetime.Singleton).As<IJsonProxy>();
            
            // editor
            var windowSO = AssetDatabase.LoadAssetAtPath<Window.SO>("Assets/Editor/ScriptableObject/Window/SOData/WindowSO.asset");
            builder.RegisterInstance(windowSO).As<Window.ISO>();
            builder.Register<Window.Config>(Lifetime.Singleton).As<Window.IConfig>();
            builder.Register<Window.Proxy>(Lifetime.Singleton).As<Window.IProxy>();
            
            var windowBuildSO = AssetDatabase.LoadAssetAtPath<Window.BuildSO>("Assets/Editor/ScriptableObject/Window/SOData/WindowBuildSO.asset");
            builder.RegisterInstance(windowBuildSO).As<Window.IBuildSO>();
            builder.Register<Window.BuildConfig>(Lifetime.Singleton).As<Window.IBuildConfig>();
            builder.Register<Window.BuildProxy>(Lifetime.Singleton).As<Window.IBuildProxy>();

            var windowExcelSO = AssetDatabase.LoadAssetAtPath<Window.ExcelSO>("Assets/Editor/ScriptableObject/Window/SOData/WindowExcelSO.asset");
            builder.RegisterInstance(windowExcelSO).As<Window.IExcelSO>();
            builder.Register<Window.ExcelConfig>(Lifetime.Singleton).As<Window.IExcelConfig>();
            builder.Register<Window.ExcelProxy>(Lifetime.Singleton).As<Window.IExcelProxy>();
            
            var windowScanSO = AssetDatabase.LoadAssetAtPath<Window.ScanSO>("Assets/Editor/ScriptableObject/Window/SOData/WindowScanSO.asset");
            builder.RegisterInstance(windowScanSO).As<Window.IScanSO>();
            builder.Register<Window.ScanConfig>(Lifetime.Singleton).As<Window.IScanConfig>();
            builder.Register<Window.ScanProxy>(Lifetime.Singleton).As<Window.IScanProxy>();
            
            var windowOtherSO = AssetDatabase.LoadAssetAtPath<Window.OtherSO>("Assets/Editor/ScriptableObject/Window/SOData/WindowOtherSO.asset");
            builder.RegisterInstance(windowOtherSO).As<Window.IOtherSO>();
            builder.Register<Window.OtherConfig>(Lifetime.Singleton).As<Window.IOtherConfig>();
            builder.Register<Window.OtherProxy>(Lifetime.Singleton).As<Window.IOtherProxy>();
            
            
            builder.Register<Window.IExcelConfig>(Lifetime.Singleton).As<Window.IExcelConfig>();
            
            builder.UseEntryPoints(Lifetime.Singleton, entryPoints =>
            {
                entryPoints.Add<AssetBundleProxy>();
                entryPoints.OnException(ex => ex.FrameError());
            });
        }
    }
}
