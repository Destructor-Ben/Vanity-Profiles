using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace VanityProfiles.Content.UI;
// TODO - refactor
internal class UIProfileElement : UIPanel
{
    public object Text;
    public Action OnClick;
    public Asset<Texture2D> Icon;
    public VanityProfile? Profile;

    public bool IsSelected => Profile != null && VanitySystem.CurrentProfile == Profile;

    public UIProfileElement(object text, Action onClick = null, Asset<Texture2D> icon = null, VanityProfile? profile = null)
    {
        Text = text;
        OnClick = onClick;
        Icon = icon;
        Profile = profile;

        if (profile is VanityProfile p && !p.IsNone)
            Text = p.Name;
        if (Text == null || Text is string s && (string.IsNullOrEmpty(s) || string.IsNullOrWhiteSpace(s)))
            Text = Util.GetTextValue("UI.Unknown");

        Width.Set(0, 1f);
        Height.Set(35, 0f);
        BackgroundColor = UICommon.DefaultUIBlueMouseOver;

        if (Icon != null)
        {
            var iconImage = new UIImage(icon)
            {
                VAlign = 0.5f,
            };
            Append(iconImage);
        }

        var uiText = Text is LocalizedText localizedText ? new UIText(localizedText) : new UIText(Text as string);
        uiText.VAlign = 0.5f;
        uiText.Left.Set(icon != null ? icon.Width() + 10 : 0, 0f);
        Append(uiText);
    }

    public override void LeftClick(UIMouseEvent evt)
    {
        OnClick();
        SoundEngine.PlaySound(SoundID.MenuTick);
        base.LeftClick(evt);
    }

    // TODO - stop text and icon from making the mouse events trigger multiple times
    public override void MouseOver(UIMouseEvent evt)
    {
        SoundEngine.PlaySound(SoundID.MenuTick);
        base.MouseOver(evt);
    }

    public override void Update(GameTime gameTime)
    {
        BackgroundColor = IsSelected ? UICommon.DefaultUIBlue : UICommon.DefaultUIBlueMouseOver;
        BorderColor = IsMouseHovering ? UICommon.DefaultUIBorderMouseOver : UICommon.DefaultUIBorder;

        base.Update(gameTime);
    }
}
