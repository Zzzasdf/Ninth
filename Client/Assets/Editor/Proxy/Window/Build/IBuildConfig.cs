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
        EnumTypeSubscribe<int> IntEnumTypeSubscribe { get; }
        CommonSubscribe<Enum, string> StringCommonSubscribe { get; }
        CommonSubscribe<Enum, int> IntCommonSubscribe { get; }
        CommonSubscribe<BuildSettingsMode, BuildConfig.BuildSettings> TabCommonSubscribe { get; }
    }
}