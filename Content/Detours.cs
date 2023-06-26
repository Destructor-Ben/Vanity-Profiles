using Terraria.Audio;
using Terraria.GameInput;
using VanityProfiles.Content.UI;

namespace VanityProfiles.Content;
internal class Detours : ModSystem
{
    private bool wasHoveringLastFrame = false;

    public override void Load()
    {
        // Vanity UI button toggle
        On_Main.DrawDefenseCounter += delegate (On_Main.orig_DrawDefenseCounter orig, int inventoryX, int inventoryY)
        {
            orig(inventoryX, inventoryY);

            // Getting values
            var texture = Util.GetTexture("VanityButton");
            var position = new Vector2(inventoryX - 10 - 47 - 47 - 14, inventoryY + 10 - texture.Height());
            bool hovering = Utils.CenteredRectangle(position, texture.Size() * 0.5f).Contains(Main.MouseScreen.ToPoint());

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
                    UISystem.Instance.VanityUIOpen = !UISystem.Instance.VanityUIOpen;
                    SoundEngine.PlaySound(UISystem.Instance.VanityUIOpen ? SoundID.MenuOpen : SoundID.MenuClose);
                }

                // Hover texture and string
                texture = Util.GetTexture("VanityButtonHover");
                Main.instance.MouseText(Util.GetTextValue("UI.VanityProfiles"));
            }
            else
            {
                wasHoveringLastFrame = false;
            }

            // Drawing
            Main.spriteBatch.Draw(texture.Value, Utils.CenteredRectangle(position, texture.Size()), Color.White);
        };

        // TODO - detour player drawing
    }
}
