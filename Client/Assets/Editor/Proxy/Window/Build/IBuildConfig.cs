using System.Collections.Generic;
using Ninth.Utility;

namespace Ninth.Editor
{
    public interface IBuildConfig : IJson
    {
        SubscriberCollect<List<string>> StringListSubscriber { get; }
        SubscriberCollect<string> StringSubscriber { get; }
        SubscriberCollect<int> IntSubscriber { get; }
        BuildConfig.BuildSettings BuildSettings { get; }
    }
}