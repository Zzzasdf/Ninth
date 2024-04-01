using System;
using UnityEngine;

namespace Ninth.HotUpdate
{
    [CreateAssetMenu(fileName = "PreLoadAssetsSO", menuName = "HotUpdateConfig/PreLoadAssetsSO")]
    public class PreLoadAssets : ScriptableObject
    {
        public TextAsset ViewAssetConfig;
    }
}
