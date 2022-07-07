using System;
using UnityEngine;


public class MovingPlatformActivateOnEnter : MovingPlatform {
    protected override void OnTriggerEnter(Collider other) {
        base.OnTriggerEnter(other);

        if (other.CompareTag("Player")) isOn = true;
    }
}