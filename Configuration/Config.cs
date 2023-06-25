using Terraria.ModLoader.Config;

#pragma warning disable CS0649
namespace VanityProfiles.Configuration;
internal class Config : ModConfig
{
    public static Config Instance => ModContent.GetInstance<Config>();
    public override ConfigScope Mode => ConfigScope.ClientSide;

    public bool TODO;
}
#pragma warning restore CS0649
