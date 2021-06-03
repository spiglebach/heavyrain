using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBlock : MonoBehaviour {
    [SerializeField] private Sprite[] fallRemainderDisplaySprites;
    private FallingObject currentFallingObject;
    private SpriteRenderer _spriteRenderer;
    private int spriteIndex;

    protected virtual void Start() {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        DisableRemainingStepDisplay();
    }

    public bool FallAndStopIfNecessary() {
        if (currentFallingObject) {
            currentFallingObject.Fall();
        }
        spriteIndex--;
        if (spriteIndex < 0) {
            currentFallingObject.Grounded();
            DisableRemainingStepDisplay();
            return true;
        }
        if (spriteIndex < fallRemainderDisplaySprites.Length) {
            _spriteRenderer.sprite = fallRemainderDisplaySprites[spriteIndex];
        }
        return false;
    }

    public void SetFallingObject(FallingObject fallingObject) {
        currentFallingObject = fallingObject;
        fallingObject.SetPlatform(this);
        spriteIndex = Mathf.RoundToInt(fallingObject.transform.position.y) - 2;
        if (spriteIndex >= 0 && spriteIndex < fallRemainderDisplaySprites.Length) {
            _spriteRenderer.sprite = fallRemainderDisplaySprites[spriteIndex];
        }
    }

    public void RemoveFallingObject() {
        currentFallingObject = null;
        DisableRemainingStepDisplay();
    }

    private void DisableRemainingStepDisplay() {
        _spriteRenderer.sprite = null;
    }

}
