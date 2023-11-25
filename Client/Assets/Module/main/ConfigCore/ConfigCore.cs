using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor.Animations;
using UnityEngine;
using System;

namespace Ninth
{
    public partial class GameEntry
    {
        [Serializable]
        public class ConfigCore
        {
            public UTF8Encoding Encoding = new UTF8Encoding(false);
            public AssetConfig AssetConfig;
            public NameConfig NameConfig;
            public PlatformConfig PlatformConfig { get; }
            public PathConfig PathConfig { get; }
            public PlayerPrefsConfing PlayerPrefsConfing { get; }

            public ConfigCore()
            {
                //Encoding = new UTF8Encoding(false);
                //AssetConfig = GetSO<AssetConfig>("AssetConfigSO");  
                //NameConfig = GetSO<NameConfig>("NameConfigSO"); 
                //PlatformConfig = new PlatformConfig(); 
                //PathConfig = new PathConfig(AssetConfig, PlatformConfig, NameConfig);
                //PlayerPrefsConfing = new PlayerPrefsConfing();
            }

            public T GetSO<T>(string soName) where T: UnityEngine.Object
            {
                return Resources.Load<T>(string.Format("SOData/{0}", soName));
            }
        }
    }
}