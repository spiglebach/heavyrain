public class Health : FallingObject {
    public override void ApplyEffect(Player player) {
        player.AddHealth();
    }
}
