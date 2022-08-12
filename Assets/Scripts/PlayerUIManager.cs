using System;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour {
      private Inventory inventory;
      public CrystalColor currentCrystal = CrystalColor.NONE;
      
      public Image currentCrystalImage;
      [Tooltip("RED, BLUE, GREEN, YELLOW, ORANGE, PURPLE, ALL")]
      public List<Sprite> crystalSprites = new();

      [Tooltip("Basic, Navigating, Three")]
      public List<Sprite> waspsAvatars = new();

      public TextMeshProUGUI currentPointsText;
      public TextMeshProUGUI maxPointsText;
      public GameObject heartPrefab;
      public GameObject heartsList;

      public TMP_Text platformVersionText;
      public GameObject waspInfo;
      public TMP_Text waspCount;
      public Image waspAvatar;

      private void Start() {
            inventory = FindObjectOfType<Inventory>();
            
            currentPointsText.text = $"0";
            maxPointsText.text = $"{GameManager.Instance.maxPoints}";
            HandleCrystal(CrystalColor.NONE);
            GameManager.Instance.eventManager.OnCrystalChange += HandleCrystal;
            GameManager.Instance.eventManager.OnPointPickup += HandlePoints;

            var projectName = Application.productName;
            var platform = Application.platform.ToString();
            var version = Application.version;

            platformVersionText.richText = true;
            platformVersionText.text = $"<b>{projectName}</b> - {platform} - {version}";

            UpdateWaspUI();
      }

      public void UpdateWaspUI() {
            waspCount.text = inventory.wasps.ToString();
            SetWaspAvatar();
      }

      private void Update() {
            waspInfo.SetActive(inventory.wasps != 0);
            waspCount.text = inventory.wasps.ToString();
      }

      public void SetWaspAvatar() {
            switch (inventory.currentWaspType) {
                  case WaspType.BASIC:
                        waspAvatar.sprite = waspsAvatars[0];
                        break;
                  case WaspType.NAVIGATING:
                        waspAvatar.sprite = waspsAvatars[1];
                        break;
                  case WaspType.THREE:
                        waspAvatar.sprite = waspsAvatars[2];
                        break;
                  default:
                        throw new ArgumentOutOfRangeException();
            }
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