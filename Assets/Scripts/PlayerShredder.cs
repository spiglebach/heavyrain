using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShredder : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        var player = other.GetComponent<Player>();
        if (!player) return;
        player.FellToDeath();
    }
}
