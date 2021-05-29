using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    // Cached object references
    private Rainer _rainer;
    private MeshRenderer _meshRenderer;
    private SphereCollider _collider;

    // Turn based movement variables
    private bool moved;
    private bool waited;
    private bool frozen;
    private int freezeTurnsLeft;
    private Vector3 movementOrigin;
    private Vector3 movementTarget;
    private Vector3 previousMoveDirection;
    private DirectionalMovement[] directionalMovements = {
        new DirectionalMovement(Direction.LEFT, new Vector3(1, 0, 0), KeyCode.D),
        new DirectionalMovement(Direction.DOWN, new Vector3(0, 0, -1), KeyCode.S),
        new DirectionalMovement(Direction.RIGHT, new Vector3(-1, 0, 0), KeyCode.A),
        new DirectionalMovement(Direction.UP, new Vector3(0, 0, 1), KeyCode.W)
    };
    
    // Smoothed movement transition variables
    [SerializeField] private float freezeDurationInSeconds = .5f;
    private float currentStepProgression = 0;
    
    // Score
    private int score;
    [SerializeField] private Text scoreDisplay;
    
    // Health
    private int health;
    [SerializeField] private Sprite[] healthSprites;
    [SerializeField][Range(1, 5)] private int maxHealth = 3;
    [SerializeField] private Image healthDisplay;

    private void Start() {
        _rainer = FindObjectOfType<Rainer>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _collider = GetComponent<SphereCollider>();
        health = maxHealth;
        DisplayScore();
        DisplayHealth();
    }

    void Update() {
        if (moved) {
            currentStepProgression += Time.deltaTime;
            var percent = currentStepProgression / Rainer.transitionTimeInSeconds;
            transform.position = Vector3.Lerp(movementOrigin, movementTarget, percent);
            if (currentStepProgression >= Rainer.transitionTimeInSeconds) {
                transform.position = movementTarget;
                moved = false;
                ClearMovement();
                currentStepProgression = 0;
                EnableCollider();
            }
            return;
        }
        if (frozen) {
            currentStepProgression += Time.deltaTime;
            if (currentStepProgression >= freezeDurationInSeconds) {
                freezeTurnsLeft--;
                if (freezeTurnsLeft <= 0) {
                    frozen = false;
                    _meshRenderer.material.color = Color.white;
                } else {
                    _rainer.Fall();
                }
                currentStepProgression = 0;
            }
            return;
        }
        if (waited) {
            currentStepProgression += Time.deltaTime;
            if (currentStepProgression >= Rainer.transitionTimeInSeconds) {
                waited = false;
                currentStepProgression = 0;
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

    private void EnableCollider() {
        _collider.enabled = true;
    }
    
    private void DisableCollider() {
        _collider.enabled = false;
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
        freezeTurnsLeft = 2;
        _rainer.Fall();
    }

    private void Wait() {
        _rainer.Fall();
        waited = true;
        currentStepProgression = 0;
    }

    void TakeStep(Vector3 direction) {
        moved = true;
        currentStepProgression = 0;
        movementOrigin = transform.position;
        movementTarget = movementOrigin + direction;
        previousMoveDirection = direction;
        DisableCollider();
        _rainer.Fall();
    }

    public void Slip() {
        var slipDirection = previousMoveDirection;
        if (IsPlatformPresentInDirection(slipDirection)) {
            TakeStep(slipDirection);
        }
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
        _rainer.ObjectPickedUp(fallingObject);
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
        healthDisplay.sprite = healthSprites[health - 1];
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
