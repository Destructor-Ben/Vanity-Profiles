using Terraria.DataStructures;
using Terraria.ModLoader.IO;

namespace VanityProfiles.Content;
internal class VanityPlayer : ModPlayer
{
    public VanityProfile CurrentProfile = VanityProfile.None;
    public PlayerFieldCache OriginalPlayerData = new();

    public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
    {
        // Storing player fields like hair
        OriginalPlayerData.GetFromPlayer(Player);

        // Applying the vanity
        CurrentProfile.ApplyToPlayer(ref drawInfo, Player);
    }

    public override void PreUpdate()
    {
        // Resetting player fields like hair
        OriginalPlayerData.ApplyToPlayer(Player);
    }

    // Saving and loading current profile 
    public override void SaveData(TagCompound tag)
    {
        // Save GUID if it the profile isn't null
        if (CurrentProfile != VanityProfile.None)
            tag.Add("CurrentProfile", CurrentProfile.ID.ToString());
    }

    public override void LoadData(TagCompound tag)
    {
        // Try to get the current profiles ID and find all profiles that have that ID
        tag.TryGet("CurrentProfile", out string ID);
        var profiles = VanitySystem.VanityProfiles.Where(p => p.ID == new Guid(ID));

        // If a profile exists set the current profile to it
        if (profiles.Any())
            CurrentProfile = profiles.First();
    }
}
