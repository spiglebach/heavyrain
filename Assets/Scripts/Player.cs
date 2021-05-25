using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField] private float progressionDurationInSeconds = .5f;
    private float currentStepProgression = 0;
    
    private bool moved = false; // todo use to determine if a step was taken
    private Vector3 movementOrigin;
    private Vector3 movementTarget;
    private DirectionalMovement[] directionalMovements = new[] {
        new DirectionalMovement("RIGHT", new Vector3(1, 0, 0), KeyCode.D),
        new DirectionalMovement("DOWN", new Vector3(0, 0, -1), KeyCode.S),
        new DirectionalMovement("LEFT", new Vector3(-1, 0, 0), KeyCode.A),
        new DirectionalMovement("UP", new Vector3(0, 0, 1), KeyCode.W)
    };

    private Rainer rainer;

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
                if (CheckDirection(movement.Direction)) {
                    TakeStep(movement.Direction);
                    return;
                }
            }
        }
    }

    void TakeStep(Vector3 direction) {
        moved = true;
        currentStepProgression = 0;
        movementOrigin = transform.position;
        movementTarget = movementOrigin + direction;
    }

    bool CheckDirection(Vector3 direction) {
        var rayStart = transform.position + direction;
        Vector3 down = Vector3.down;
        return Physics.Raycast(rayStart, down, 2, LayerMask.GetMask("Platforms"));
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
