using System;

namespace Ninth.Utility
{
    // 需要转换成 Json 的类要继承 IJson
    public interface IJson
    {
    
    }
    
    public interface IJsonConfig
    {
        SubscriberCollect<string> StringSubscriber { get; }
    }
}
