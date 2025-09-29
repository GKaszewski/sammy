using UniRx;

namespace Sammy.Interfaces
{
    public interface IPlayerDataService
    {
        IReadOnlyReactiveProperty<int> Wasps { get; }
        IReadOnlyReactiveProperty<CrystalColor> CurrentCrystal { get; }
        int Points { get; }
        int Lives { get; }
        WaspType CurrentWaspType { get; }
        
        void CollectPoints(int amount);
        void AddWasps(int amount);
        void SetWasps(int amount);
        void DecreaseWasps();
        void ChangeCrystal(CrystalColor newColor);
        void TakeDamage(int damage);
        void ResetHealth();
    }
}