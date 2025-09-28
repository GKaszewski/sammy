using UniRx;
using UnityEngine;

public class Inventory : MonoBehaviour {
    [SerializeField] public int points;
    public WaspType currentWaspType;

    public Transform crystalSpawnPosition;
    [HideInInspector]
    public CrystalColor previousColor;
    
    private readonly ReactiveProperty<int> _wasps = new(10);
    public IReadOnlyReactiveProperty<int> Wasps => _wasps;
    public ReactiveProperty<CrystalColor> ReactiveCrystalInfo { get; } = new(CrystalColor.NONE);

    private void Start() {
        ReactiveCrystalInfo.Subscribe(color => GameManager.Instance.eventManager.ChangeCrystal(color));
    }
    
    public void CollectPoints(int pointsToAdd) {
        points += pointsToAdd;
        if (points >= GameManager.Instance.maxPoints) {
            ReactiveCrystalInfo.Value = CrystalColor.MULTI;
            AudioManager.Instance.Play("Won");
        }
        GameManager.Instance.eventManager.PickPointUp(points);
    }

    public void DecreaseWasps() {
        if (_wasps.Value <= 0) return;
       _wasps.Value--;
    }
    
    public void AddWasps(int amount) {
        _wasps.Value += amount;
        if (_wasps.Value > 99) _wasps.Value = 99;
        AudioManager.Instance.Play("wasp pickup");
    }
    
    public void SetWasps(int amount) {
        _wasps.Value = amount;
        if (_wasps.Value > 99) _wasps.Value = 99;
        AudioManager.Instance.Play("wasp pickup");
    }
}