using System;

public class EventManager {
    public event Action<CrystalColor> OnCrystalPickup;
    public event Action<int> OnPointPickup;
    public event Action<CrystalColor> OnCrystalChange;
    public event Action OnLionAttack;
    public event Action<LionAI, AIState> OnLionAIStateChange;
    public event Action<LionAI> OnLionSpawn;
    public event Action<LionAI> OnLionDestroy;

    public void PickCrystalUp(CrystalColor color) => OnCrystalPickup?.Invoke(color);
    public void PickPointUp(int points) => OnPointPickup?.Invoke(points);
    public void ChangeCrystal(CrystalColor color) => OnCrystalChange?.Invoke(color);
    public void LionAttack() => OnLionAttack?.Invoke();
    public void LionAIStateChange(LionAI ai, AIState state) => OnLionAIStateChange?.Invoke(ai, state);
    public void SpawnLion(LionAI ai) => OnLionSpawn?.Invoke(ai);
    public void DestroyLion(LionAI ai) => OnLionDestroy?.Invoke(ai);
}