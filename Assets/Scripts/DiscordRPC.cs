using System;
using UnityEngine;
using Discord;
using UnityEngine.SceneManagement;

public class DiscordRPC : MonoBehaviour {
    private static DiscordRPC _instance;
    public static DiscordRPC Instance => _instance;

    private Discord.Discord _discord;
    private long _startTime;

    public string Details { get; set;  } = "In the main menu";
    public string State { get; set;  } = "In the main menu";
    public string LargeImage { get; set; } = "logo";
    public string LargeText { get; set;  } = "In the main menu";


    private void Awake() {
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else {
            Destroy(this);
        }


        _startTime = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        _discord = new Discord.Discord(1070512056987566150, (System.UInt64) CreateFlags.Default);
        UpdateRpc();

        SceneManager.sceneLoaded += (scene, mode) => {
            Details = $"In {scene.name}";
            State = $"Having fun ;)";
            LargeText = $"In {scene.name}";
            UpdateRpc();
            
            Debug.Log($"Discord RPC updated to {scene.name}");
        };
    }

    public void UpdateRpc() {
        var activityManager = _discord.GetActivityManager();
        var activity = new Activity {
            Details = Details,
            State = State,
            Assets = {
                LargeImage = LargeImage,
                LargeText = LargeText,
            },
            Timestamps = {
                Start = _startTime
            }
        };
        
        activityManager.UpdateActivity(activity, (res) => {
            if (res == Result.Ok) {
                Debug.Log("Discord RPC updated successfully");
            }
            else {
                Debug.Log("Discord RPC failed to update");
            }
        });
    }

    private void Update() {
        _discord.RunCallbacks();
    }
    
    private void OnApplicationQuit() {
        _discord.Dispose();
    }
}