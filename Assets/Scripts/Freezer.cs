using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freezer : Hazard {
    [SerializeField] private int minFreezeTurns = 1;
    [SerializeField] private int maxFreezeTurns = 1;

    public override void ApplyEffect(Player player) {
        player.Freeze();
    }
}
