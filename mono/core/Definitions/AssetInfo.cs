using System.Collections.Generic;

namespace mono.core.Definitions
{

    public enum AtlasName
    {
        NoAtlas,
        Tileset1,
        Player,
        Platform1,
        Parallax1,
        Parallax2,
        Start,
        SplashScreen
    }

    public static class AssetInfo
    {
        // Assets
        static public Dictionary<AtlasName, AtlasInfo> infos = new Dictionary<AtlasName, AtlasInfo>
        {
            {AtlasName.Tileset1, new AtlasInfo("Graphics/tileset", 32, 32, 0, 0)},
            {AtlasName.Player, new AtlasInfo("Graphics/player", 64, 128, 0, 0)},
            {AtlasName.Parallax1, new AtlasInfo("Graphics/paysage", 5760, 720, 0, 0, -190)},
            {AtlasName.Parallax2, new AtlasInfo("Graphics/avantPlan", 8630, 720, 0, 0, -190)},
            {AtlasName.Start, new AtlasInfo("Graphics/start", 1280, 720, 0, 0)},
            {AtlasName.SplashScreen, new AtlasInfo("Graphics/title", 1080, 720, 0, 0)}
        };
    }
}
