using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freezer : Hazard {
    public override void ApplyEffect(Player player) {
        base.ApplyEffect(player);
        player.Freeze();
    }
}
