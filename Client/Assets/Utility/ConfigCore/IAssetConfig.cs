using System.Collections.ObjectModel;

namespace Ninth.Utility
{
    public interface IAssetConfig
    {
        // 运行环境
        Environment RuntimeEnv();
        
        // 模块列表Url
        string Url();
    }
}