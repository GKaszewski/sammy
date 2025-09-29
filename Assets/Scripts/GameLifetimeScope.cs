using Sammy;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    public PlayerUIManager playerUIManager;
    public AIManager aiManager;
    public EffectsManager effectsManager;
    public Inventory inventory;
    public PlayerInput playerInput;
    public PlayerMover playerMover;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<EventManager>(Lifetime.Singleton);
        builder.Register<CrystalManager>(Lifetime.Singleton);
     
        builder.RegisterInstance(playerInput);
        builder.RegisterInstance(playerMover);
        builder.RegisterInstance(inventory);
        builder.RegisterInstance(aiManager);
        builder.RegisterInstance(effectsManager);
        builder.RegisterInstance(playerUIManager);
        
        Debug.Log("Registered GameLifetimeScope dependencies.");
    }
}