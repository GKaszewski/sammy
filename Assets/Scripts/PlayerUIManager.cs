using System;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour {
      public CrystalColor currentCrystal = CrystalColor.NONE;
      
      public Image currentCrystalImage;
      [Tooltip("RED, BLUE, GREEN, YELLOW, ORANGE, PURPLE, ALL")]
      public List<Sprite> crystalSprites = new List<Sprite>();

      public TextMeshProUGUI currentPointsText;
      public TextMeshProUGUI maxPointsText;
      public GameObject heartPrefab;
      public GameObject heartsList;

      private void Start() {
            currentPointsText.text = $"0";
            maxPointsText.text = $"{GameManager.Instance.maxPoints}";
            HandleCrystal(CrystalColor.NONE);
            GameManager.Instance.eventManager.OnCrystalChange += HandleCrystal;
            GameManager.Instance.eventManager.OnPointPickup += HandlePoints;
      }

      private void HandlePoints(int points) {
            currentPointsText.text = $"{points}";
      }

      private void HandleCrystal(CrystalColor color) {
            currentCrystal = color;
            HandleCrystalImage();
      } 
      
      private void HandleCrystalImage() {
            if (currentCrystal != CrystalColor.NONE) currentCrystalImage.enabled = true;
            switch (currentCrystal) {
                  case CrystalColor.RED:
                        currentCrystalImage.sprite = crystalSprites[0];
                        break;
                  case CrystalColor.BLUE:
                        currentCrystalImage.sprite = crystalSprites[1];
                        break;
                  case CrystalColor.GREEN:
                        currentCrystalImage.sprite = crystalSprites[2];
                        break;
                  case CrystalColor.YELLOW:
                        currentCrystalImage.sprite = crystalSprites[3];
                        break;
                  case CrystalColor.ORANGE:
                        currentCrystalImage.sprite = crystalSprites[4];
                        break;
                  case CrystalColor.PURPLE:
                        currentCrystalImage.sprite = crystalSprites[5];
                        break;
                  case CrystalColor.MULTI:
                        currentCrystalImage.sprite = crystalSprites[6];
                        break;
                  default:
                        currentCrystalImage.sprite = null;
                        currentCrystalImage.enabled = false;
                        break;
            }
      }
}