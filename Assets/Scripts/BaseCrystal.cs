using System.Collections.Generic;
using KBCore.Refs;
using UnityEngine;

public class BaseCrystal : MonoBehaviour {
    [SerializeField, Child] private MeshRenderer renderer;
    
    [Tooltip("RED, BLUE, GREEN, YELLOW, ORANGE, PURPLE")]
    [SerializeField] private List<Material> crystalMaterials = new();

    public CrystalColor color;
    
    protected void Start() {
        HandleMaterial();
    }

    public void HandleMaterial() {
        switch (color) {
            case CrystalColor.RED:
                renderer.material = crystalMaterials[0];
                break;
            case CrystalColor.BLUE:
                renderer.material = crystalMaterials[1];
                break;
            case CrystalColor.GREEN:
                renderer.material = crystalMaterials[2];
                break;
            case CrystalColor.YELLOW:
                renderer.material = crystalMaterials[3];
                break;
            case CrystalColor.ORANGE:
                renderer.material = crystalMaterials[4];
                break;
            case CrystalColor.PURPLE:
                renderer.material = crystalMaterials[5];
                break;
        }
    }
}