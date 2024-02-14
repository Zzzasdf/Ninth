using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System;

namespace Ninth
{
        [Serializable]
        public class ConfigCore
        {
            public AssetConfig AssetConfig;
            public NameConfig NameConfig;
            public PlayerSettings PlayerSettings { get; }
            public PathConfig PathConfig { get; }
            public PlayerPrefsConfig PlayerPrefsConfig { get; }

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