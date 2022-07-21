using System;

public class EventManager {
    public event Action<CrystalColor> OnCrystalPickup;
    public event Action<int> OnPointPickup;
    public event Action<CrystalColor> OnCrystalChange;
    public event Action OnLionAttack;

    public void PickCrystalUp(CrystalColor color) => OnCrystalPickup?.Invoke(color);
    public void PickPointUp(int points) => OnPointPickup?.Invoke(points);
    public void ChangeCrystal(CrystalColor color) => OnCrystalChange?.Invoke(color);
    public void LionAttack() => OnLionAttack?.Invoke();
}