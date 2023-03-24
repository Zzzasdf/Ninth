using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth.HotUpdate
{
    public static class JsonPathConfig
    {
        private static Dictionary<Type, string> m_Map;

        public static bool IsExist<T>() where T : IModel
        {
            return m_Map.ContainsKey(typeof(T));
        }

        public static string Get<T>() where T : IModel
        {
            return m_Map[typeof(T)];
        }

        static JsonPathConfig()
        {
            m_Map = new Dictionary<Type, string>();

            AllMode();

            switch (SOCore.GetGlobalConfig().AssetMode)
            {
                case AssetMode.NonAB:
                    {
                        NonABMode();
                        break;
                    }
                case AssetMode.LocalAB:
                    {
                        LocalABMode();
                        break;
                    }
                case AssetMode.RemoteAB:
                    {
                        RemoteABMode();
                        break;
                    }
            }
        }

        private static void AllMode()
        {
            m_Map.Add(typeof(ModelTest), Application.streamingAssetsPath + "/ASD/MMM.json");
            m_Map.Add(typeof(LocalLoadConfig), PathConfig.LoadConfigInLocalInStreamingAssetPath());
        }

        private static void NonABMode()
        {

        }

        private static void LocalABMode()
        {
            m_Map.Add(typeof(RemoteLoadConfig), PathConfig.LoadConfigInRemoteInStreamingAssetPath());
        }

        private static void RemoteABMode()
        {
            m_Map.Add(typeof(RemoteLoadConfig), PathConfig.LoadConfigInRemoteInPersistentDataPath());
        }
    }
}
