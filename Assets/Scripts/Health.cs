public class Health : FallingObject {
    public override void ApplyEffect(Player player) {
        base.ApplyEffect(player);
        player.AddHealth();
    }
}
