using System;
using System.IO;
using UnityEngine;

public class ScreenshotTaker : MonoBehaviour {
    public string defaultPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
    public string screenshotPath = "screenshots";
    public KeyCode screenshotKey;
    private void Update() {
        if (Input.GetKeyDown(screenshotKey)) {
            var dirPath = $"{defaultPath}\\{screenshotPath}";
            if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
            var date = DateTime.UtcNow.ToString("o");
            date = date.Replace("/","-");
            date = date.Replace(" ","_");
            date = date.Replace(":","-");
            date = date.Replace(".","-");
            var filePath = $"{defaultPath}\\{screenshotPath}\\screenshot-{date}.png";
            ScreenCapture.CaptureScreenshot(filePath);
        }
    }
}