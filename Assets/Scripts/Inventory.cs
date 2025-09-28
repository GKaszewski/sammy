using UniRx;
using UnityEngine;

public class Inventory : MonoBehaviour {
    [SerializeField] public int points;
    public WaspType currentWaspType;

    public GameObject crystalPrefab;
    public Transform crystalSpawnPosition;
    [HideInInspector]
    public CrystalColor previousColor;
    
    private readonly ReactiveProperty<int> _wasps = new(10);
    public IReadOnlyReactiveProperty<int> Wasps => _wasps;
    public ReactiveProperty<CrystalColor> ReactiveCrystalInfo { get; } = new(CrystalColor.NONE);

    private void Start() {
        ReactiveCrystalInfo.Subscribe(color => GameManager.Instance.eventManager.ChangeCrystal(color));
    }
    
    public void CollectPoints(int _points) {
        points += _points;
        if (points >= GameManager.Instance.maxPoints) {
            ReactiveCrystalInfo.Value = CrystalColor.MULTI;
            AudioManager.Instance.Play("Won");
        }
        GameManager.Instance.eventManager.PickPointUp(points);
    }
    
    private Crystal SpawnCrystal(CrystalColor color, Vector3 position) {
        AudioManager.Instance.Play("crystal spawn");
        var newCrystal = Instantiate(crystalPrefab, position, Quaternion.identity).GetComponent<Crystal>();
        newCrystal.color = color;
        newCrystal.HandleMaterial();

        return newCrystal;
    }

    public void DecreaseWasps() {
        if (_wasps.Value <= 0) return;
       _wasps.Value--;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) {
        if (!hit.collider.CompareTag("Crystal")) return;
        var newCrystal = hit.gameObject.GetComponent<Crystal>();
        if (!newCrystal) return;
        AudioManager.Instance.Play("crystal pickup");
        if (ReactiveCrystalInfo.Value != CrystalColor.NONE && ReactiveCrystalInfo.Value != CrystalColor.MULTI) {
            var newColor = Crystal.Mix(ReactiveCrystalInfo.Value, newCrystal.color);
            if (newColor == CrystalColor.NONE) {
                var spawnedCrystal = SpawnCrystal(ReactiveCrystalInfo.Value, crystalSpawnPosition.position + (transform.forward * 2f));
                spawnedCrystal.player = transform;
                spawnedCrystal.ApplyForce();
            }
            else
                AudioManager.Instance.Play("crystal mix");

            ReactiveCrystalInfo.Value = newColor != CrystalColor.NONE ? newColor : newCrystal.color;
        }
        else if (ReactiveCrystalInfo.Value == CrystalColor.NONE) {
            ReactiveCrystalInfo.Value = newCrystal.color;
        }
        GameManager.Instance.eventManager.PickCrystalUp(ReactiveCrystalInfo.Value);
        newCrystal.Die();
    }

    public void DropCrystal() {
        if (ReactiveCrystalInfo.Value == CrystalColor.NONE) return;
        var spawnedCrystal = SpawnCrystal(ReactiveCrystalInfo.Value, crystalSpawnPosition.position + (transform.forward * 2f));
        spawnedCrystal.player = transform;
        spawnedCrystal.ApplyForceUp();
        AudioManager.Instance.Play("crystal spawn");
        ReactiveCrystalInfo.Value = CrystalColor.NONE;
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