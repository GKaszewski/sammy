using System.Collections.Generic;
using KBCore.Refs;
using Sammy;
using UnityEngine;

public class BaseCrystal : MonoBehaviour {
    [SerializeField] private CrystalColorApplier crystalColorApplier;
    
    public CrystalColor color;
    
    public void HandleMaterial() {
        crystalColorApplier.ApplyMaterial(color);
    }
}