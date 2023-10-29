using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.UI;
using Terraria.UI;
using TerraUtil.UI.Elements;

namespace VanityProfiles.Content.UI;
// TODO: allow profiles to be selected but not equipped
internal class UIVanity : UIWindow
{
    public static UIVanity Instance => ModContent.GetInstance<UIVanity>();

    protected override LocalizedText WindowTitle => Util.GetText("UI.VanityProfiles");

    public UIElement RightSide;
    public UIElement LeftSide;

    public UIText ProfileName;
    public UIList ProfileList;

    protected override void CreateUI()
    {
        #region Basic Setup

        base.CreateUI();

        Visible = false;
        ShouldUpdate = false;

        LeftSide = new UIElement
        {
            Height = { Percent = 1f },
            Width = { Percent = 0.6f },
            PaddingBottom = 12,
            PaddingRight = 12,
        };
        Content.Append(LeftSide);

        RightSide = new UIElement
        {
            Height = { Percent = 1f },
            Width = { Percent = 0.4f },
            HAlign = 1f,
            PaddingBottom = 12,
            PaddingLeft = 12,
        };
        Content.Append(RightSide);

        var divider = new UIDivider(horizontal: false)
        {
            HAlign = 0.6f,
        };
        Content.Append(divider);

        #endregion

        #region Left Side

        PopulateCustomization();

        #endregion

        #region Right Side

        // Profile preview
        ProfileName = new UIText(Util.GetText("UI.NoProfileSelected"))
        {
            HAlign = 0.5f,
        };
        RightSide.Append(ProfileName);

        var preview = new UIProfilePreview
        {
            Top = { Pixels = 25 },
            HAlign = 0.5f,
        };
        RightSide.Append(preview);

        // Profile management
        float profileManagementHeight = 400;

        var profilesText = new UIText(Util.GetText("UI.Profiles"))
        {
            Top = { Pixels = -profileManagementHeight, Percent = 1f },
        };
        RightSide.Append(profilesText);

        var profilesPanel = new UIPanel
        {
            Top = { Pixels = -profileManagementHeight + 25, Percent = 1f },
            Width = { Percent = 1f },
            Height = { Pixels = profileManagementHeight - 20 },
            BackgroundColor = UICommon.MainPanelBackground,
        };
        RightSide.Append(profilesPanel);

        var profilesScrollbar = new UIScrollbar
        {
            Height = { Percent = 1f },
            HAlign = 1f,
        };
        profilesPanel.Append(profilesScrollbar);

        ProfileList = new UIList
        {
            Width = { Pixels = -25, Percent = 1f },
            Height = { Percent = 1f },
            ListPadding = 6f,
        };
        ProfileList.SetScrollbar(profilesScrollbar);
        PopulateProfiles();
        profilesPanel.Append(ProfileList);

        #endregion
    }

    public override void SafeUpdate(GameTime gameTime)
    {
        base.SafeUpdate(gameTime);

        // Updating the profile text
        if (VanitySystem.CurrentProfile.IsNone)
            ProfileName.SetText(Util.GetText("UI.NoProfileSelected"));
        else
            ProfileName.SetText(VanitySystem.CurrentProfile.Name ?? Util.GetTextValue("UI.Unknown"));
    }

    // Refreshes the left (customization UI) UI
    public void PopulateCustomization()
    {
        // Reset children
        LeftSide.RemoveAllChildren();

        // If it is none don't recreate the UI
        if (VanitySystem.CurrentProfile.IsNone)
            return;

        // Creating the UI
        // TODO: make the UI
        // Name
        // Delete
        // Equip
        // Apply in multiplayer
        // Set as default profile for new players
        // Rest of the fields

        var deleteButton = new UIImageButton(TextureAssets.Item[5000])
        {

        };
        deleteButton.OnLeftClick += delegate (UIMouseEvent evt, UIElement listeningElement)
        {
            DeleteProfile(VanitySystem.CurrentProfile);
        };
        LeftSide.Append(deleteButton);

        Recalculate();
    }

    // Populates the profiles list
    // TODO: refactor
    public void PopulateProfiles()
    {
        // Clear existing elements
        ProfileList.Clear();

        // Add new profile and no profile elements
        ProfileList.Add(new UIProfileElement(Util.GetText("UI.NewProfile"), CreateProfile, Util.GetTexture("UI.NewProfile"), profile: null));
        ProfileList.Add(new UIProfileElement(Util.GetText("UI.NoProfile"), () => SetProfile(VanityProfile.None), profile: VanityProfile.None));

        // Null check
        if (VanitySystem.VanityProfiles == null)
            return;

        // Add profiles
        foreach (var profile in VanitySystem.VanityProfiles)
        {
            ProfileList.Add(new UIProfileElement(profile.Name, () => SetProfile(profile), profile: profile));
        }

        Recalculate();
    }

    public void CreateProfile()
    {
        var profile = new VanityProfile().Register();
        SetProfile(profile);
        PopulateProfiles();
    }

    public void DeleteProfile(VanityProfile profile)
    {
        profile.Delete();
        SetProfile(VanityProfile.None);
        PopulateProfiles();
    }

    public void SetProfile(VanityProfile profile)
    {
        profile.SetAsCurrent();
        PopulateCustomization();
    }
}
