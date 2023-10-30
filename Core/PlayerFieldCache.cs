namespace VanityProfiles.Core;

public class PlayerFieldCache
{
    // TODO: add fields
    public int Hair = -1;

    public void GetFromPlayer(Player player)
    {
        Hair = player.hair;
    }

    public void ApplyToPlayer(Player player)
    {
        if (Hair != -1)
            player.hair = Hair;
    }
}
