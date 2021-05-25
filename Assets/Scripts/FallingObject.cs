using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    public void Fall() {
        transform.Translate(Vector3.down, Space.World);
    }
}
