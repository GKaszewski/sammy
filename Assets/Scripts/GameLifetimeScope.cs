using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    public PlayerUIManager playerUIManager;
    public AIManager aiManager;
    public EffectsManager effectsManager;
    public Inventory inventory;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(playerUIManager);
        builder.RegisterInstance(aiManager);
        builder.RegisterInstance(effectsManager);
        builder.RegisterInstance(inventory);
        
        builder.Register<EventManager>(Lifetime.Singleton);
    }
}