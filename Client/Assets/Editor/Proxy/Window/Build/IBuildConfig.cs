using System;
using System.Collections.Generic;
using Ninth.Utility;

namespace Ninth.Editor
{
    public interface IBuildConfig
    {
        Subscriber<Enum, List<string>> StringListSubscriber { get; }
        Subscriber<Enum, string> StringSubscriber { get; }
        TypeSubscriber<int> IntSubscriber { get; }
        BuildConfig.BuildSettings BuildSettings { get; }
    }
}