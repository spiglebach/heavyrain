public class Spike : Hazard {
    public override void ApplyEffect(Player player) {
        base.ApplyEffect(player);
        player.RemoveHealth();
    }
}
