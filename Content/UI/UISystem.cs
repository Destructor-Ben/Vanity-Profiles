using Terraria.UI;

namespace VanityProfiles.Content.UI;
internal class UISystem : ModSystem
{
    public static UISystem Instance => ModContent.GetInstance<UISystem>();

    public UserInterface VanityInterface;
    public UIVanity VanityState;

    public bool VanityUIOpen
    {
        get => VanityInterface.CurrentState != null;
        set => VanityInterface.SetState(value ? VanityState : null);
    }

    public override void Load()
    {
        if (!Main.dedServ)
        {
            VanityState = new UIVanity();
            VanityState.Activate();
            VanityInterface = new UserInterface();
        }
    }

    public override void Unload()
    {
        VanityInterface = null;
        VanityState = null;
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        int index = layers.FindIndex(layer => layer.Name == "Vanilla: Mouse Text");
        if (index == -1)
            return;

        layers.Insert(index, new LegacyGameInterfaceLayer(
            "VanityProfiles: VanityInterface",
            delegate
            {
                VanityInterface.Draw(Main.spriteBatch, null);
                return true;
            },
            InterfaceScaleType.UI
        ));
    }

    public override void UpdateUI(GameTime gameTime)
    {
        VanityInterface.Update(gameTime);
    }
}
