using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilDrop : Hazard {
    [SerializeField] private GameObject fallingLook;
    [SerializeField] private GameObject groundedLook;

    private void Start() {
        fallingLook.SetActive(true);
        groundedLook.SetActive(false);
    }

    public override void Grounded() {
        fallingLook.SetActive(false);
        groundedLook.SetActive(true);
    }

    public override void ApplyEffect(Player player) {
        player.Slip();
    }
}
