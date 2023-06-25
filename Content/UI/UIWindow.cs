using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader.UI;

namespace VanityProfiles.Content.UI;
internal class UIWindow : UIPanel
{
    public UIElement Content;
    public UIElement TitleBar;

    private Action closeWindowAction;
    private Vector2 dragOffset;
    private bool dragging;

    public UIWindow(LocalizedText titleText, Action closeWindow)
    {
        closeWindowAction = closeWindow;
        BackgroundColor = UICommon.MainPanelBackground;

        // Title bar
        TitleBar = new UIElement
        {
            Width = { Percent = 1f },
            Height = { Pixels = 30 },
        };
        Append(TitleBar);

        // Content
        Content = new UIElement
        {
            Width = { Percent = 1f },
            Height = { Pixels = -20, Percent = 1f },
            Top = { Pixels = 30 },
            PaddingTop = 12,
        };
        Append(Content);

        // Close button
        var closeButton = new UIImageButton(VanitySystem.CloseButton)
        {
            HAlign = 1f,
        };
        closeButton.OnLeftClick += CloseWindow;
        TitleBar.Append(closeButton);

        // Title
        var title = new UIText(titleText, 0.5f, large: true);
        TitleBar.Append(title);

        // Divider
        var divider = new UIDivider
        {
            VAlign = 1f,
        };
        TitleBar.Append(divider);
    }

    public override void Update(GameTime gameTime)
    {
        // Stop weapons from being able to be used while the window is being hovered over
        if (IsMouseHovering)
            Main.LocalPlayer.mouseInterface = true;

        // Dragging
        var dimensions = GetDimensions();

        if (!Main.mouseLeft)
            dragging = false;

        if (Main.mouseLeft && TitleBar.ContainsPoint(Main.MouseScreen) || dragging)
        {
            dragging = true;

            if (dragOffset == Vector2.Zero)
                dragOffset = Main.MouseScreen - dimensions.Position();

            var newPos = Main.MouseScreen - dragOffset;
            Left.Set(newPos.X, 0f);
            Top.Set(newPos.Y, 0f);
            HAlign = 0f;
            VAlign = 0f;
        }
        else
        {
            dragOffset = Vector2.Zero;
        }

        base.Update(gameTime);
    }

    private void CloseWindow(UIMouseEvent evt, UIElement listeningElement)
    {
        closeWindowAction();
    }
}
