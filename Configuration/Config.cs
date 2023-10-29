using Terraria.ModLoader.Config;

namespace VanityProfiles.Configuration;

#pragma warning disable CS0649

public class Config : ModConfig
{
    public static Config Instance => ModContent.GetInstance<Config>();
    public override ConfigScope Mode => ConfigScope.ClientSide;

    public bool TODO;
}
