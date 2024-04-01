using System.Collections.Generic;
using Ninth.Utility;

namespace Ninth.HotUpdate
{
    public enum VIEW_HIERARCHY
    {
        Bg,
        Main,
        Frame,
        Tip,
        Pop
    }

    public interface IViewConfig
    {
        public ViewAssetConfig ViewAssetConfig { get; }
    }
}
