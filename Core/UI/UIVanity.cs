using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.UI;
using Terraria.UI;
using TerraUtil.UI.Elements;

namespace VanityProfiles.Core.UI;

// TODO: allow profiles to be selected but not equipped
public class UIVanity : UIWindow
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
            BackgroundColor = UICommon.DefaultUIBlueMouseOver,
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
        profilesPanel.Append(ProfileList);

        PopulateProfiles();

        #endregion
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        // Updating the profile text
        if (VanitySystem.CurrentProfile.IsNone)
            ProfileName.SetText(Util.GetText("UI.NoProfileSelected"));
        else
            ProfileName.SetText(VanitySystem.CurrentProfile.Name ?? Util.GetTextValue("UI.Unknown"));
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

    // Refreshes the left (customization UI) UI
    // TODO: finish
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
    public void PopulateProfiles()
    {
        ProfileList.Clear();

        // New profile button
        var newProfileButton = CreateButton(Util.GetText("UI.NewProfile"), CreateProfile, null);
        var newIcon = new UIImage(Util.GetTexture("UI.NewProfile", loadAsync: false))
        {
            VAlign = 0.5f,
        };
        newProfileButton.Append(newIcon);
        ProfileList.Add(newProfileButton);

        // No profile button
        var noProfileButton = CreateButton(Util.GetText("UI.NoProfile"), () => SetProfile(VanityProfile.None), VanityProfile.None);
        ProfileList.Add(noProfileButton);

        if (VanitySystem.VanityProfiles == null)
            return;

        // Add elements for all profiles
        foreach (var profile in VanitySystem.VanityProfiles)
        {
            // TODO: add rename and delete buttons
            // TODO: the entire lists needs to be sorted with UISOrtableElements
            var button = CreateButton(profile.Name, () => SetProfile(profile), profile);
            ProfileList.Add(button);
        }

        Recalculate();

        // Creates the ui element used for the buttons in the list
        // TODO: tweak a bit later
        UIButton<T> CreateButton<T>(T text, Action onClick, VanityProfile? profile)
        {
            var button = new UIButton<T>(text)
            {
                Height = { Pixels = 35 },
                Width = { Percent = 1f },
                UseAltColors = () => VanitySystem.CurrentProfile != profile,

                AltPanelColor = UICommon.MainPanelBackground,
                AltHoverPanelColor = UICommon.MainPanelBackground * (1 / 0.8f),
                ClickSound = SoundID.MenuTick,
            };
            button.OnLeftClick += (_, _) => onClick();

            return button;
        }
    }
}
