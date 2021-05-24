using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private const float MOVEMENT_DEADZONE = 0.4f;
    private bool moved = false;
    private bool aPressed = false;
    private bool dPressed = false;
    private bool wPressed = false;
    private bool sPressed = false;
    void Start() {
        
    }

    void Update() {
        MoveLeft();
        MoveRight();
        MoveUp();
        MoveDown();
    }

    void MoveLeft() {
        if (aPressed) {
            if (Input.GetKeyUp(KeyCode.A)) {
                aPressed = false;
            }
            return;
        }
        aPressed = Input.GetKeyDown(KeyCode.A);

        if (aPressed) {
            var direction = Vector3.left;
            if (CheckDirection(direction)) {
                transform.Translate(direction);
            }
        }
    }
    void MoveRight() {
        if (dPressed) {
            if (Input.GetKeyUp(KeyCode.D)) {
                dPressed = false;
            }
            return;
        }
        dPressed = Input.GetKeyDown(KeyCode.D);

        if (dPressed) {
            var direction = Vector3.right;
            if (CheckDirection(direction)) {
                transform.Translate(direction);
            }
        }
    }
    
    void MoveUp() {
        if (wPressed) {
            if (Input.GetKeyUp(KeyCode.W)) {
                wPressed = false;
            }
            return;
        }
        wPressed = Input.GetKeyDown(KeyCode.W);

        if (wPressed) {
            var direction = new Vector3(0, 0, 1);
            if (CheckDirection(direction)) {
                transform.Translate(direction);
            }
        }
    }
    
    void MoveDown() {
        if (sPressed) {
            if (Input.GetKeyUp(KeyCode.S)) {
                sPressed = false;
            }
            return;
        }
        sPressed = Input.GetKeyDown(KeyCode.S);

        if (sPressed) {
            var direction = new Vector3(0, 0, -1);
            if (CheckDirection(direction)) {
                transform.Translate(direction);
            }
        }
    }

    bool CheckDirection(Vector3 direction) {
        var rayStart = transform.position + direction;
        Vector3 down = Vector3.down;
        return Physics.Raycast(rayStart, down, 2, LayerMask.GetMask("Platforms"));
    }
}
