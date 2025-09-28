using System;
using UnityEngine;

namespace Sammy
{
    [Serializable]
    public struct ColorMaterialPair
    {
        public CrystalColor color;
        public Material[] materialsToApply;
    }
}