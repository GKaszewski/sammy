using System;
using System.Collections;
using KBCore.Refs;
using Sammy.Interfaces;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {
    [SerializeField] private float speed;
    [SerializeField] private float changeDirectionDelay;
    [SerializeField, Self] private Rigidbody rb;
    
    protected bool isMoving;

    private Transform _destinationTarget, _departTarget;
    private CharacterController _cc;
    private Vector3 _oldPosition;
    private Vector3 _delta;
    private float _startTime;
    private float _movingTime;
    private float _journeyLength;
    private bool _isWaiting;
    private int _currentWaypoint;
    private IActivationStrategy _activationStrategy;

    public Transform[] waypoints;
    public bool isOn = false;
    
    protected void Start() {
        TryGetComponent(out _activationStrategy);
        
        _departTarget = waypoints[0];
        _destinationTarget = waypoints[1];

        _startTime = 0f;
        _journeyLength = Vector3.Distance(_departTarget.position, _destinationTarget.position);
    }

    protected void Update() {
        if (isMoving) _movingTime += Time.deltaTime;
        _delta = transform.position - _oldPosition;

        if (isOn) {
            _oldPosition = transform.position;
            Move();
            if (_cc) _cc.Move( _delta);
        }
        else isMoving = false;
    }

    private void LateUpdate() {
        _oldPosition = transform.position;
    }
    
    private void Move() {
        if (!_isWaiting) {
            if (Vector3.Distance(transform.position, _destinationTarget.position) > 0.01f) {
                isMoving = true;
                var distCovered = (_movingTime - _startTime) * speed;
                var fractionOfJourney = distCovered / _journeyLength;
                rb.MovePosition(Vector3.Lerp(_departTarget.position, _destinationTarget.position, fractionOfJourney));
            }
            else {
                _isWaiting = true;
                isMoving = false;
                StartCoroutine(ChangeDelay());
            }
        }
    }

    private void ChangeDestination() {
        var previous = _currentWaypoint;
        _currentWaypoint = (_currentWaypoint + 1) % waypoints.Length;
        _departTarget = waypoints[previous];
        _destinationTarget = waypoints[_currentWaypoint];
    }

    private IEnumerator ChangeDelay() {
        yield return new WaitForSeconds(changeDirectionDelay);
        ChangeDestination();
        _startTime = 0f;
        _movingTime = 0f;
        _journeyLength = Vector3.Distance(_departTarget.position, _destinationTarget.position);
        _isWaiting = false;
    }

    protected virtual void OnTriggerEnter(Collider other) {
        // if (other.CompareTag("Player")) {
        //     _cc = other.gameObject.GetComponent<CharacterController>();
        // }

        if (_activationStrategy != null && _activationStrategy.ShouldActivate(other))
        {
            isOn = true;
        }
    }

    protected virtual void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            _cc = null;
        }
    }
}