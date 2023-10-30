namespace VanityProfiles.Core;
internal class VanitySystem : ModSystem
{
    public static VanitySystem Instance => ModContent.GetInstance<VanitySystem>();
    public static VanityProfile CurrentProfile
    {
        get => Main.LocalPlayer.TryGetModPlayer<VanityPlayer>(out var player) ? player.CurrentProfile : VanityProfile.None;
        set => Main.LocalPlayer.GetModPlayer<VanityPlayer>().CurrentProfile = value;
    }

    public static List<VanityProfile> VanityProfiles { get; private set; }

    public override void Load()
    {
        VanityProfiles = new();
        LoadProfiles();
    }

    public override void Unload()
    {
        VanityProfiles = null;
    }

    // Loads all of the vanity profiles under Terraria/VanityProfiles
    // TODO: implement
    public static void LoadProfiles()
    {

    }

    // Saves all of the vanity profiles to json files
    // TODO: finish. make async, can't access the files
    // TODO: save individual profiles with a button
    public static void SaveProfiles()
    {
        // Folder path and making the directory
        string path = Path.Join(Main.SavePath, "VanityProfiles");
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        // Saving each of the profiles by getting their json string and saving that
        foreach (var profile in VanityProfiles)
        {
            string name = Path.Join(path, profile.ID.ToString());
            string contents = profile.ToString();
            File.WriteAllText(path, contents);
        }
    }
}
