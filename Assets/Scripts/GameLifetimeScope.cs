using Sammy;
using Sammy.Controllers;
using Sammy.Handlers;
using Sammy.Interfaces;
using Sammy.Services;
using Sammy.Views;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<IInputService, LegacyInputService>(Lifetime.Singleton);
        builder.Register<IGameStateService, GameStateService>(Lifetime.Singleton);
        builder.Register<IPlayerDataService, PlayerDataService>(Lifetime.Singleton);
        
        builder.Register<EventManager>(Lifetime.Singleton);
        builder.Register<CrystalManager>(Lifetime.Singleton);
        
        builder.RegisterComponentInHierarchy<PlayerView>().As<IPlayerView>();
        builder.RegisterComponentInHierarchy<UIView>().As<IUIView>();
        
        builder.Register<IPlayerMovementHandler, PlayerMovementHandler>(Lifetime.Scoped);
        builder.Register<IPlayerCombatHandler, PlayerCombatHandler>(Lifetime.Scoped);
        
        builder.RegisterEntryPoint<PlayerController>();
        builder.RegisterEntryPoint<UIPresenter>();
    }
}