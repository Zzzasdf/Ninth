using System;
using Ninth.Editor.Window;
using Ninth.Utility;
using VContainer;

namespace Ninth.Editor
{
    public class JsonConfig: BaseJsonConfig
    {
        [Inject]
        public JsonConfig(IPathProxy pathProxy)
        {
            typeSubscribe = new GenericsSubscribe<Type, string?>()
                .Subscribe(typeof(Tab), "");
        }
    }
}