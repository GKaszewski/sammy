using System;
using UnityEngine;

public class MovingCrystal : BaseCrystal {
    private float currentTime;
    private float distance = 0f;
    private Vector3 startPosition;
    private Vector3 target;
    private float movementTime;
    
    private void Update() {
        var direction = target - transform.position;
        direction = direction.normalized * movementTime * Time.deltaTime;
        distance = Vector3.Distance(transform.position, target);
        if (distance > 0.01f) transform.position += direction;
    }

    public void SetDestination(Vector3 start, Vector3 destination, float time) {
        startPosition = start;
        target = destination;
        movementTime = time;
    }
}