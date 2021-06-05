using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour {
    [SerializeField] private AudioClip pickupClip;
    private PlatformBlock _platformBlock;
    private bool falling;
    private float currentTransitionTime;
    private Vector3 originalPosition;
    private Vector3 targetPosition;

    public virtual void ApplyEffect(Player player) {
        if (pickupClip) {
            var soundSettings = FindObjectOfType<SoundSettings>();
            float volume = 1f;
            if (soundSettings) {
                volume = soundSettings.GetCompositeSfxVolume();
            }
            AudioSource.PlayClipAtPoint(pickupClip, transform.position, volume);
        }
    }

    public virtual void Grounded() {
        
    }
    
    public void Fall() {
        falling = true;
        currentTransitionTime = 0;
        originalPosition = transform.position;
        targetPosition = originalPosition + Vector3.down;
    }

    private void Update() {
        if (falling) {
            currentTransitionTime += Time.deltaTime;
            float progress = currentTransitionTime / Rainer.transitionTimeInSeconds;
            transform.position = Vector3.Lerp(originalPosition, targetPosition, progress);
            if (currentTransitionTime >= Rainer.transitionTimeInSeconds) {
                transform.position = targetPosition;
                falling = false;
            }
        }
    }

    public void SetPlatform(PlatformBlock platform) {
        _platformBlock = platform;
    }

    public void RemoveFromPlatform() {
        _platformBlock.RemoveFallingObject();
    }

    public PlatformBlock GetPlatform() {
        return _platformBlock;
    }
}
