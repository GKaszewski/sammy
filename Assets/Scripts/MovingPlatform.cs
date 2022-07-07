using System;
using System.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {
    [SerializeField] private float speed;
    [SerializeField] private float changeDirectionDelay;
    
    protected bool isMoving;

    private Transform destinationTarget, departTarget;
    private float startTime;
    private float movingTime;
    private float journeyLength;
    private bool isWaiting;
    private int currentWaypoint;

    public Transform[] waypoints;
    public bool isOn = false;
    
    protected void Start() {
        departTarget = waypoints[0];
        destinationTarget = waypoints[1];

        startTime = 0f;
        journeyLength = Vector3.Distance(departTarget.position, destinationTarget.position);
    }

    protected void Update() {
        if (isMoving) movingTime += Time.deltaTime;
    }

    private void FixedUpdate() {
        if (isOn) Move();
        else isMoving = false;
    }

    private void Move() {
        if (!isWaiting) {
            if (Vector3.Distance(transform.position, destinationTarget.position) > 0.01f) {
                isMoving = true;
                var distCovered = (movingTime - startTime) * speed;
                var fractionOfJourney = distCovered / journeyLength;
                transform.position = Vector3.Lerp(departTarget.position, destinationTarget.position, fractionOfJourney);
            }
            else {
                isWaiting = true;
                isMoving = false;
                StartCoroutine(ChangeDelay());
            }
        }
    }

    private void ChangeDestination() {
        var previous = currentWaypoint;
        currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        departTarget = waypoints[previous];
        destinationTarget = waypoints[currentWaypoint];
    }

    private IEnumerator ChangeDelay() {
        yield return new WaitForSeconds(changeDirectionDelay);
        ChangeDestination();
        startTime = 0f;
        movingTime = 0f;
        journeyLength = Vector3.Distance(departTarget.position, destinationTarget.position);
        isWaiting = false;
    }

    protected virtual void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) other.transform.parent = transform;
    }

    protected virtual void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) other.transform.parent = null;
    }
}