using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    // Cached object references
    private Rainer rainer;

    // Turn based movement variables
    private bool moved;
    private bool waited;
    private Vector3 movementOrigin;
    private Vector3 movementTarget;
    private DirectionalMovement[] directionalMovements = {
        new DirectionalMovement("RIGHT", new Vector3(1, 0, 0), KeyCode.D),
        new DirectionalMovement("DOWN", new Vector3(0, 0, -1), KeyCode.S),
        new DirectionalMovement("LEFT", new Vector3(-1, 0, 0), KeyCode.A),
        new DirectionalMovement("UP", new Vector3(0, 0, 1), KeyCode.W)
    };
    
    // Smoothed movement transition variables
    [SerializeField] private float progressionDurationInSeconds = .05f;
    private float currentStepProgression = 0;
    
    private void Start() {
        rainer = FindObjectOfType<Rainer>();
    }

    void Update() {
        if (moved) {
            currentStepProgression += Time.deltaTime;
            var percent = currentStepProgression / progressionDurationInSeconds;
            transform.position = Vector3.Lerp(movementOrigin, movementTarget, percent);
            if (currentStepProgression >= progressionDurationInSeconds) {
                transform.position = movementTarget;
                moved = false;
                rainer.Fall();
            }
            return;
        }
        if (waited) {
            currentStepProgression += Time.deltaTime;
            if (currentStepProgression >= progressionDurationInSeconds) {
                waited = false;
                rainer.Fall();
            }
            return;
        }
        for (var i = 0; i < directionalMovements.Length; i++) {
            var movement = directionalMovements[i];
            if (movement.IsPressed()) {
                foreach (var keyCode in movement.KeyCodes) {
                    if (Input.GetKeyUp(keyCode)) {
                        movement.SetPressed(false);
                    }
                }
                continue;
            }

            bool isPressed = false;
            foreach (var keyCode in movement.KeyCodes) {
                isPressed = isPressed || Input.GetKeyDown(keyCode);
            }

            movement.SetPressed(isPressed);

            if (movement.IsPressed()) {
                if (IsPlatformPresentInDirection(movement.Direction)) {
                    TakeStep(movement.Direction);
                    return;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            Wait();
        }
    }

    private void Wait() {
        waited = true;
        currentStepProgression = 0;
    }

    void TakeStep(Vector3 direction) {
        moved = true;
        currentStepProgression = 0;
        movementOrigin = transform.position;
        movementTarget = movementOrigin + direction;
    }

    private bool IsPlatformPresentInDirection(Vector3 direction) {
        var rayStart = transform.position + direction;
        Vector3 down = Vector3.down;
        return Physics.Raycast(
            rayStart, 
            down,
            2,
            LayerMask.GetMask("Platforms"));
    }

    private void OnTriggerEnter(Collider other) {
        var fallingObject = other.gameObject.GetComponent<FallingObject>();
        if (!fallingObject) {
            return;
        }
        // todo catch or trigger hazard
        rainer.ObjectPickedUp(fallingObject);
        fallingObject.RemoveFromPlatform();
        Destroy(other.gameObject);
    }
}

class DirectionalMovement {
    private bool isPressed;
    private Vector3 direction;
    private KeyCode[] keyCodes;

    public DirectionalMovement(string label, Vector3 direction, params KeyCode[] keyCodes) {
        this.direction = direction;
        this.keyCodes = keyCodes;
    }

    public bool IsPressed() {
        return isPressed;
    }

    public void SetPressed(bool pressed) {
        isPressed = pressed;
    }

    public Vector3 Direction => direction;

    public KeyCode[] KeyCodes => keyCodes;
}
