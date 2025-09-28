using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    public PlayerUIManager playerUIManager;
    public AIManager aiManager;
    public EffectsManager effectsManager;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(playerUIManager);
        builder.RegisterInstance(aiManager);
        builder.RegisterInstance(effectsManager);
        
        builder.Register<EventManager>(Lifetime.Singleton);
    }
}