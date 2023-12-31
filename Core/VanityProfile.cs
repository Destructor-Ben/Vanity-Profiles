﻿using System.Text.Json.Serialization;
using Terraria.DataStructures;

namespace VanityProfiles.Core;

[Serializable]
public struct VanityProfile : IEquatable<VanityProfile>
{
    [JsonIgnore]
    public string Name = Util.GetTextValue("DefaultProfileName");
    [JsonIgnore]
    public Guid ID { get; init; }

    [JsonIgnore]
    public bool IsNone
    {
        readonly get => isNone;

        init
        {
            isNone = value;
            ID = new Guid();// TODO: default id for none
        }
    }
    [JsonIgnore]
    private bool isNone;

    #region Vanity Fields

    // TODO: add all fields
    public int Hair = -1;

    #endregion

    public static readonly VanityProfile None = new() { IsNone = true };

    public VanityProfile()
    {
        ID = Guid.NewGuid();
    }

    // Adds to the list of vanity profiles
    public readonly VanityProfile Register()
    {
        VanitySystem.VanityProfiles.Add(this);
        return this;
    }

    // Sets as the current vanity profile
    public readonly VanityProfile SetAsCurrent()
    {
        VanitySystem.CurrentProfile = this;
        return this;
    }

    // Destroys the current profile by removing it from the vanity profiles list and setting itself equal to VanityProfile.None
    public void Delete()
    {
        if (VanitySystem.VanityProfiles.Contains(this))
            VanitySystem.VanityProfiles.Remove(this);
        this = None;
    }

    // Applies the effects of the vanity profile to a player
    public readonly void ApplyToPlayer(ref PlayerDrawSet drawInfo, Player player)
    {
        if (IsNone)
            return;

        if (Hair != -1)
            player.hair = Hair;

        // TODO: temp
        if (Main.IsInTheMiddleOfLoadingSettings)
            Main.NewText(drawInfo);
    }

    // Equailty and whatnor
    public static bool operator ==(VanityProfile a, object b) => a.Equals(b);

    public static bool operator !=(VanityProfile a, object b) => !a.Equals(b);

    public override readonly int GetHashCode()
    {
        return ID.GetHashCode();
    }

    public override readonly bool Equals(object obj)
    {
        return obj is VanityProfile profile && Equals(profile);
    }

    public readonly bool Equals(VanityProfile other)
    {
        return (IsNone && other.IsNone) || ID == other.ID;
    }
}
