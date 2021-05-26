using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBlock : MonoBehaviour {
    [SerializeField] private Sprite[] fallRemainderDisplaySprites;
    private SpriteRenderer _spriteRenderer;
    private int objectHeight;
    
    void Start() {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _spriteRenderer.sprite = null;
    }

    public void ObjectFallingFromHeight(int height) {
        if (height < fallRemainderDisplaySprites.Length) {
            objectHeight = height;
            _spriteRenderer.sprite = fallRemainderDisplaySprites[height];
        }
    }

    public void Fall() {
        objectHeight--;
        if (objectHeight <= 0) {
            _spriteRenderer.sprite = null;
            return;
        }
        _spriteRenderer.sprite = fallRemainderDisplaySprites[objectHeight];
    }

}
