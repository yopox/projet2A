using System;
using System.Collections.Generic;

namespace mono.core.Definitions
{
    public static class AssetInfo
    {
        // Assets
        static public Dictionary<AtlasName, AtlasInfo> infos = new Dictionary<AtlasName, AtlasInfo>()
        {
            {AtlasName.Tileset1, new AtlasInfo("Graphics/tileset", 32, 32, 0, 0)},
            {AtlasName.Player, new AtlasInfo("Graphics/hero", 64, 128, 0, 0)},
            {AtlasName.Platform1, new AtlasInfo("", 0, 0, 0, 0)},
        };
    }
}
