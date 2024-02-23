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
        CommonSubscribe<BuildDirectoryRoot, string> StringCommonSubscribe { get; }
        CommonSubscribe<BuildVersion, int> IntCommonSubscribe { get; }
    }
}