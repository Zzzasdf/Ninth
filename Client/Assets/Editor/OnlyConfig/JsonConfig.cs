using System;
using Ninth.Editor.Window;
using Ninth.Utility;
using VContainer;
using NullReferenceException = System.NullReferenceException;

namespace Ninth.Editor
{
    public class JsonConfig: BaseJsonConfig
    {
        [Inject]
        public JsonConfig(IWindowConfig windowConfig)
        {
            enumTypeSubscribe = new EnumTypeSubscribe<string?>()
                .Subscribe<Tab>(windowConfig.GetEnumType<Tab>());

            commonSubscribe = new CommonSubscribe<Enum, string?>()
                .Subscribe(Tab.Build, windowConfig.Get(Tab.Build)?.path ?? throw new NullReferenceException())
                .Subscribe(Tab.Excel, windowConfig.Get(Tab.Excel)?.path ?? throw new NullReferenceException())
                .Subscribe(Tab.Scan, windowConfig.Get(Tab.Scan)?.path ?? throw new NullReferenceException());
        }
    }
}