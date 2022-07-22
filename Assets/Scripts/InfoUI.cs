using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class InfoUI : MonoBehaviour {
  public GameObject infoUI;
  public TMP_Text infoText;
  [TextArea]
  public string data;

  private bool isShowing = false;

  public Vector3 showPosition;
  public Vector3 hidePosition;
  public float showMovementTime = 1.25f;
  public float hideMovementTime = 0.75f;
  public float showTime = 4f;
  public float delayBeforeShowing = 0.5f;

  public bool showInfo = false;
  public bool showInfoOnStart = false;

  private async void Start() {
    if (!showInfo) Hide();
    if (showInfoOnStart) {
      await Task.Delay(TimeSpan.FromSeconds(delayBeforeShowing));
      Show();
    }
    infoText.text = data;
  }

  private void Update() {
    if (showInfo && !isShowing) {
      if (Input.GetButtonDown("HideInfoUI")) Hide();
      Show();
    }
  }

  private async void Show() {
    isShowing = true;
    LeanTween.cancel(infoUI);
    transform.localPosition = showPosition;
    LeanTween.scale(infoUI, Vector3.one, showMovementTime).setEase(LeanTweenType.easeOutElastic);
    await Task.Delay(TimeSpan.FromSeconds(showTime));
    Hide();
  }

  private void Hide() {
    transform.localPosition = hidePosition;
    LeanTween.cancel(infoUI);
    LeanTween.scale(infoUI, Vector3.zero, hideMovementTime).setEase(LeanTweenType.easeInElastic).setOnComplete(() => {
      isShowing = false;
      showInfo = false;
    });
  }
}
