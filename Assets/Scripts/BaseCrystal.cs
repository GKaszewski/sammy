using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseCrystal : MonoBehaviour {
    public CrystalColor color;
    public MeshRenderer renderer;
    
    [Tooltip("RED, BLUE, GREEN, YELLOW, ORANGE, PURPLE")]
    public List<Material> crystalMaterials = new List<Material>();

    protected void Start() {
        renderer = GetComponentInChildren<MeshRenderer>();
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