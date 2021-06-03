using UnityEngine;

public class PlayerShredder : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        var player = other.GetComponent<Player>();
        if (!player) return;
        if (player.IsGameOver()) {
            Destroy(player.gameObject);
        } else {
            player.FellToDeath();
        }
    }
}
