using Terraria.GameContent;

namespace VanityProfiles.Content.UI;
internal class UIDivider : UIElement
{
    public bool Horizontal;

    public UIDivider(bool horizontal = true)
    {
        Horizontal = horizontal;

        if (horizontal)
        {
            Width.Set(0, 1f);
            Height.Set(4, 0f);
        }
        else
        {
            Width.Set(4, 0f);
            Height.Set(0, 1f);
        }
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        var dimensions = GetDimensions();

        var mainRect = Horizontal ? new Rectangle((int)dimensions.X, (int)dimensions.Y, (int)dimensions.Width, 2) : new Rectangle((int)dimensions.X, (int)dimensions.Y, 2, (int)dimensions.Height);
        var secondaryRect = Horizontal ? new Rectangle((int)dimensions.X, (int)dimensions.Y + 2, (int)dimensions.Width, 2) : new Rectangle((int)dimensions.X + 2, (int)dimensions.Y, 2, (int)dimensions.Height);

        spriteBatch.Draw(TextureAssets.MagicPixel.Value, mainRect, Color.LightGray);
        spriteBatch.Draw(TextureAssets.MagicPixel.Value, secondaryRect, Color.DarkGray);// TODO - colour
    }
}
