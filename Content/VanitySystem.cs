using System.Diagnostics;

namespace VanityProfiles.Content;
internal class VanitySystem : ModSystem
{
    public static VanitySystem Instance => ModContent.GetInstance<VanitySystem>();
    public static VanityProfile CurrentProfile
    {
        get => Main.LocalPlayer.TryGetModPlayer<VanityPlayer>(out var player) ? player.CurrentProfile : VanityProfile.None;
        set => Main.LocalPlayer.GetModPlayer<VanityPlayer>().CurrentProfile = value;
    }

    public static List<VanityProfile> VanityProfiles { get; private set; }
    public static Stopwatch SaveTime;

    // Textures
    public static Asset<Texture2D> CloseButton => RequestTexture("CloseButton");
    public static Asset<Texture2D> NewProfile => RequestTexture("NewProfile");
    public static Asset<Texture2D> VanityButton => RequestTexture("VanityButton");
    public static Asset<Texture2D> VanityButtonHover => RequestTexture("VanityButtonHover");

    public override void Load()
    {
        VanityProfiles = new();
        SaveTime = new Stopwatch();
        LoadProfiles();
    }

    public override void Unload()
    {
        VanityProfiles = null;
        SaveTime = null;
    }

    // Periodically saving profiles
    public override void PostUpdateEverything()
    {
        // Turn it on if it isn't already
        if (!SaveTime.IsRunning)
            SaveTime.Start();

        // Saving ig enough time has passed
        if (SaveTime.ElapsedMilliseconds > 1000)//300000
        {
            Main.NewText("Saving Profiles");
            SaveTime.Reset();
            SaveProfiles();
        }
    }

    // Shorthand for getting textures using immediate load
    public static Asset<Texture2D> RequestTexture(string name)
    {
        return ModContent.Request<Texture2D>($"VanityProfiles/Assets/Textures/{name}", AssetRequestMode.ImmediateLoad);
    }

    // Loads all of the vanity profiles under Terraria/VanityProfiles TODO
    public static void LoadProfiles()
    {

    }

    // Saves all of the vanity profiles to json files TODO finish. make async, can't access tje files
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
