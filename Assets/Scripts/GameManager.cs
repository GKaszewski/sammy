using System;
using TouchControlsKit;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }

    public EventManager eventManager;
    public PlayerUIManager playerUIManager;
    public AIManager aiManager;

    public int maxPoints;

    public bool mobileControls = false;

    public TCKJoystick joystick;
    public TCKButton attackButton;
    public TCKButton jumpButton;
    public GameObject attackLabel;
    public GameObject jumpLabel;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
            return;
        }
        Instance = this;
        
        eventManager = new EventManager();
        var deviceType = SystemInfo.deviceType;
        if (deviceType is DeviceType.Handheld or DeviceType.Unknown || Application.platform is RuntimePlatform.Android or RuntimePlatform.IPhonePlayer) mobileControls = true;
    }

    private void Start() {
        AudioManager.instance.Play("Music");
        if (PlayerPrefs.HasKey("mobileControls")) {
            mobileControls = Convert.ToBoolean(PlayerPrefs.GetInt("mobileControls"));
        }
    }

    private void Update() {
        if (mobileControls) EnableMobileControls();
        else DisableMobileControls();
    }

    public void DisableMobileControls() {
        joystick.isActive = false;
        joystick.isVisible = false;
        attackButton.isActive = false;
        jumpButton.isActive = false;
        attackButton.isVisible = false;
        jumpButton.isVisible = false;
        jumpLabel.SetActive(false);
        attackLabel.SetActive(false);
    }

    public void EnableMobileControls() {
        joystick.isActive = true;
        joystick.isVisible = true;
        jumpButton.isActive = true;
        jumpButton.isVisible = true;
        jumpLabel.SetActive(true);
    }
}