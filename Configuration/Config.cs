#pragma warning disable CS0649
using Terraria.ModLoader.Config;

namespace VanityProfiles.Configuration;
internal class Config : ModConfig
{
    public static Config Instance => ModContent.GetInstance<Config>();
    public override ConfigScope Mode => ConfigScope.ClientSide;

    public bool TODO;
}
