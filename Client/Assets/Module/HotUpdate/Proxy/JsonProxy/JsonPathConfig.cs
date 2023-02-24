using System;
using System.Collections.Generic;

namespace Ninth.HotUpdate
{
    public partial class JsonProxy
    {
        private class JsonPathConfig
        {
            private static Dictionary<Type, string> m_Map;

            static JsonPathConfig()
            {
                m_Map = new Dictionary<Type, string>();

                AllMode();

                switch (GlobalConfig.AssetMode)
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

            public static string Get<E>() where E : IJson
            {
                return m_Map[typeof(E)];
            }
        }
    }
}
