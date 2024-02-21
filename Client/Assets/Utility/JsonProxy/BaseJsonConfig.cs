using System;

namespace Ninth.Utility
{
    public abstract class BaseJsonConfig: IJsonConfig
    {
        protected GenericsSubscribe<IJson, string> genericsSubscribe;
        protected EnumTypeSubscribe<string> enumTypeSubscribe;
        protected CommonSubscribe<Enum, string> commonSubscribe;

        GenericsSubscribe<IJson, string> IJsonConfig.GenericsSubscribe => genericsSubscribe;
        EnumTypeSubscribe<string> IJsonConfig.EnumTypeSubscribe => enumTypeSubscribe;
        CommonSubscribe<Enum, string> IJsonConfig.CommonSubscribe => commonSubscribe;
    }
}