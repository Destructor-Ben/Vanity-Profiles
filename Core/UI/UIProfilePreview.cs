using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.UI;

namespace VanityProfiles.Core.UI;

public class UIProfilePreview : UIPanel
{
    private UICharacter character;

    public UIProfilePreview()
    {
        Width.Set(100, 0f);
        Height.Set(125, 0f);
        BackgroundColor = UICommon.MainPanelBackground;

        character = new UICharacter(new Player(), hasBackPanel: false, characterScale: 2f, useAClone: false)// TODO: sizing
        {
            Width = { Percent = 1f },
            Height = { Percent = 1f },
        };
        Append(character);
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        base.DrawSelf(spriteBatch);

        character.SetAnimated(IsMouseHovering);
    }
}
