using System.Collections.Generic;
using Ninth.Utility;

namespace Ninth.Editor
{
    public interface IBuildConfig : IJson
    {
        SubscribeCollect<List<string>> StringListSubscribe { get; }
        SubscribeCollect<string> StringSubscribe { get; }
        SubscribeCollect<int> IntSubscribe { get; }
        BuildConfig.BuildSettings BuildSettings { get; }
    }
}