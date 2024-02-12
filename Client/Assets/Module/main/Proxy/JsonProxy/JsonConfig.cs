using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Ninth
{
    public enum JsonFile
    {
        Test,
    }
    
    public class JsonConfig: IJsonConfig
    {
        private readonly ReadOnlyDictionary<JsonFile, string> mapContainer;
        
        public JsonConfig()
        {
            var tempContainer = new Dictionary<JsonFile, string>();
            mapContainer = new ReadOnlyDictionary<JsonFile, string>(tempContainer);

            Subscribe(JsonFile.Test, "asdadsadsd");
            return;
            
            void Subscribe(JsonFile jsonFile, string path)
            {
                if (!tempContainer.TryAdd(jsonFile, path))
                {
                    jsonFile.FrameError("重复注册 Json: {0}");
                }
            }
        }
        
        ReadOnlyDictionary<JsonFile, string> IJsonConfig.MapContainer() => mapContainer;
    }
}
