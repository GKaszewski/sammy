using UnityEngine;

public enum WaspType {
    BASIC,
    NAVIGATING,
    THREE
}

public class WaspData : MonoBehaviour {
    public WaspType type;
    public int quantity;
    public GameObject prefab;
}