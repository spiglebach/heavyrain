using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour {
    private PlatformBlock _platformBlock;
    public void Fall() {
        transform.Translate(Vector3.down, Space.World);
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
