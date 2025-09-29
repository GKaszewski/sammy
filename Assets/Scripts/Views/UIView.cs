using System.Collections.Generic;
using Sammy.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sammy.Views
{
    public class UIView : MonoBehaviour, IUIView
    {
        [SerializeField] private TextMeshProUGUI currentPointsText;
        [SerializeField] private TextMeshProUGUI maxPointsText;
        [SerializeField] private TextMeshProUGUI waspCount;
        [SerializeField] private Image currentCrystalImage;
        [SerializeField] private List<Sprite> crystalSprites;
        
        public void UpdatePoints(int current, int max)
        {
            currentPointsText.text = current.ToString();
            maxPointsText.text = max.ToString();
        }

        public void UpdateWaspCount(int count)
        {
            waspCount.text = count.ToString();
        }
    
        public void UpdateCrystal(CrystalColor color)
        {
            if (color == CrystalColor.NONE) {
                currentCrystalImage.enabled = false;
            } else {
                currentCrystalImage.enabled = true;
                currentCrystalImage.sprite = crystalSprites[(int)color];
            }
        }
    }
}