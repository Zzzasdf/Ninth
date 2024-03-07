using System;
using System.Collections;
using System.Collections.Generic;
using Ninth.Utility;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public interface IBuildConfig : IJson
    {
        SubscribeCollect<List<string>> StringListSubscribe { get; }
        SubscribeCollect<string> StringSubscribe { get; }
        SubscribeCollect<int> IntSubscribe { get; }
        SubscribeCollect<BuildConfig.BuildSettingssss> BuildSettingsSubscribe { get; }
    }
}