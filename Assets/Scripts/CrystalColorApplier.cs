using System;
using System.Linq;
using UnityEngine;

namespace Sammy
{
    public class CrystalColorApplier : MonoBehaviour
    {
        [SerializeField] private MeshRenderer targetRenderer;
        [SerializeField] private CrystalColor crystalColor;
        [SerializeField] private ColorMaterialPair[] materialMappings = Array.Empty<ColorMaterialPair>();

        private void Start()
        {
            ApplyMaterial(crystalColor);
        }

        public void ApplyMaterial(CrystalColor color)
        {
            if (crystalColor == CrystalColor.NONE || !targetRenderer) return;
            
            var mapping = materialMappings.FirstOrDefault(m => m.color == color);
            if (mapping.materialsToApply is { Length: > 0 })
            {
                targetRenderer.materials = mapping.materialsToApply;
            }
        }
    }
}