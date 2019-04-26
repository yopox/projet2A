using System.Collections.Generic;

namespace mono.core.Definitions
{

    public enum AtlasName
    {
        Tileset1,
        Player,
        Platform1,
        Parallax1,
        Parallax2,
        Start
    }

    public static class AssetInfo
    {
        // Assets
        static public Dictionary<AtlasName, AtlasInfo> infos = new Dictionary<AtlasName, AtlasInfo>
        {
            {AtlasName.Tileset1, new AtlasInfo("Graphics/tileset", 32, 32, 0, 0)},
            {AtlasName.Player, new AtlasInfo("Graphics/player", 64, 128, 0, 0)},
            {AtlasName.Parallax1, new AtlasInfo("Graphics/test_parallaxe", 1280, 720, 0, 0)},
            {AtlasName.Parallax2, new AtlasInfo("Graphics/backerground", 1280, 720, 0, 0)},
            {AtlasName.Start, new AtlasInfo("Graphics/start", 1920, 1080, 0, 0)}
        };
    }
}
