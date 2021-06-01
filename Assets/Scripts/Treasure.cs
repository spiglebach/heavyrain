using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : FallingObject {
    [SerializeField] private int scoreAward = 100;
    public override void ApplyEffect(Player player) {
        base.ApplyEffect(player);
        player.AddToScore(scoreAward);
    }
}
