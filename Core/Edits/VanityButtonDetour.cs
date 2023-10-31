using Terraria.Audio;
using Terraria.GameInput;
using TerraUtil.Edits;
using VanityProfiles.Core.UI;

namespace VanityProfiles.Core.Edits;

public class VanityButtonDetour : Detour
{
    private bool wasHoveringLastFrame = false;

    public override void Apply()
    {
        // Vanity UI button toggle
        On_Main.DrawDefenseCounter += delegate (On_Main.orig_DrawDefenseCounter orig, int inventoryX, int inventoryY)
        {
            orig(inventoryX, inventoryY);

            var texture = Util.GetTexture("UI.VanityButton");
            var position = new Vector2(inventoryX - 10 - 47 - 47 - 14, inventoryY + 10 - texture.Height());
            bool hovering = Utils.CenteredRectangle(position, texture.Size()).Contains(Main.MouseScreen.ToPoint());

            if (hovering && !PlayerInput.IgnoreMouseInterface)
            {
                if (!wasHoveringLastFrame)
                    SoundEngine.PlaySound(SoundID.MenuTick);

                wasHoveringLastFrame = true;
                Main.LocalPlayer.mouseInterface = true;

                // Clicked
                if (Main.mouseLeft && Main.mouseLeftRelease)
                {
                    Main.mouseLeftRelease = false;
                    UIVanity.Instance.Visible = !UIVanity.Instance.Visible;
                    SoundEngine.PlaySound(UIVanity.Instance.Visible ? SoundID.MenuOpen : SoundID.MenuClose);
                }

                // Hover texture and string
                var hoverTexture = Util.GetTexture("UI.VanityButtonHover");
                Main.spriteBatch.Draw(hoverTexture.Value, Utils.CenteredRectangle(position, hoverTexture.Size()), Colors.FancyUIFatButtonMouseOver);
                Util.MouseText(Util.GetTextValue("UI.VanityProfiles"));
            }
            else
            {
                wasHoveringLastFrame = false;
            }

            // Drawing
            Main.spriteBatch.Draw(texture.Value, Utils.CenteredRectangle(position, texture.Size()), Color.White);
        };
    }
}
