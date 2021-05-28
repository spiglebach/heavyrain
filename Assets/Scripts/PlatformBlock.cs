using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBlock : MonoBehaviour {
    [SerializeField] private Sprite[] fallRemainderDisplaySprites;
    private FallingObject currentFallingObject;
    private SpriteRenderer _spriteRenderer;
    private int spriteIndex;
    
    void Start() {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _spriteRenderer.sprite = null;
    }

    public void Fall() {
        if (currentFallingObject) {
            currentFallingObject.Fall();
        }
        spriteIndex--;
        if (spriteIndex < 0) {
            _spriteRenderer.sprite = null;
            currentFallingObject = null;
            return;
        }
        _spriteRenderer.sprite = fallRemainderDisplaySprites[spriteIndex];
    }

    public void SetFallingObject(FallingObject fallingObject) {
        this.currentFallingObject = fallingObject;
        this.spriteIndex = Mathf.RoundToInt(fallingObject.transform.position.y) - 2;
        if (spriteIndex >= 0 && spriteIndex < fallRemainderDisplaySprites.Length) {
            _spriteRenderer.sprite = fallRemainderDisplaySprites[spriteIndex];
        }
    }
    public bool IsObjectFallingAbove() {
        return currentFallingObject;
    }

}
