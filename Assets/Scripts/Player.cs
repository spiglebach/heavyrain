using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    // Cached object references
    private Rainer rainer;
    private MeshRenderer _meshRenderer;

    // Turn based movement variables
    private bool moved;
    private bool waited;
    private bool frozen;
    private Vector3 movementOrigin;
    private Vector3 movementTarget;
    private DirectionalMovement[] directionalMovements = {
        new DirectionalMovement(Direction.LEFT, new Vector3(1, 0, 0), KeyCode.D),
        new DirectionalMovement(Direction.DOWN, new Vector3(0, 0, -1), KeyCode.S),
        new DirectionalMovement(Direction.RIGHT, new Vector3(-1, 0, 0), KeyCode.A),
        new DirectionalMovement(Direction.UP, new Vector3(0, 0, 1), KeyCode.W)
    };
    
    // Smoothed movement transition variables
    [SerializeField] private float progressionDurationInSeconds = .05f;
    [SerializeField] private float freezeDurationInSeconds = 1f;
    private float currentStepProgression = 0;
    
    // Score
    private int score;
    [SerializeField] private Text scoreDisplay;
    
    // Health
    private int health;
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private Text healthDisplay;

    private void Start() {
        rainer = FindObjectOfType<Rainer>();
        _meshRenderer = GetComponent<MeshRenderer>();
        DisplayScore();
    }

    void Update() {
        if (moved) {
            currentStepProgression += Time.deltaTime;
            var percent = currentStepProgression / progressionDurationInSeconds;
            transform.position = Vector3.Lerp(movementOrigin, movementTarget, percent);
            if (currentStepProgression >= progressionDurationInSeconds) {
                transform.position = movementTarget;
                moved = false;
                ClearMovement();
                currentStepProgression = 0;
                rainer.Fall();
            }
            return;
        }
        if (frozen) {
            currentStepProgression += Time.deltaTime;
            if (currentStepProgression >= freezeDurationInSeconds) {
                frozen = false;
                rainer.Fall();
                currentStepProgression = 0;
                _meshRenderer.material.color = Color.white;
            }
            return;
        }
        if (waited) {
            currentStepProgression += Time.deltaTime;
            if (currentStepProgression >= progressionDurationInSeconds) {
                waited = false;
                currentStepProgression = 0;
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

    private void ClearMovement() {
        foreach (var direction in directionalMovements) {
            direction.SetPressed(false);
        }
    }

    public void Freeze() {
        frozen = true;
        _meshRenderer.material.color = Color.blue;
        currentStepProgression = 0;
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
        rainer.ObjectPickedUp(fallingObject);
        fallingObject.RemoveFromPlatform();
        fallingObject.ApplyEffect(this);
        Destroy(other.gameObject);
    }

    public void AddToScore(int amount) {
        score += amount;
        DisplayScore();
    }

    private void DisplayScore() {
        scoreDisplay.text = score.ToString();
    }

    public void AddHealth() {
        health++;
        if (health > maxHealth) {
            health = maxHealth;
        }
        DisplayHealth();
    }

    private void DisplayHealth() {
        // todo implement
    }

    public void RemoveHealth() {
        health--;
        if (health <= 0) {
            Die();
            return;
        }
        DisplayHealth();
    }

    private void Die() {
        // todo implement
    }
}

class DirectionalMovement {
    private bool isPressed;
    private Vector3 direction;
    private KeyCode[] keyCodes;

    public DirectionalMovement(Direction label, Vector3 direction, params KeyCode[] keyCodes) {
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

enum Direction {
    LEFT,
    UP,
    RIGHT,
    DOWN
}
