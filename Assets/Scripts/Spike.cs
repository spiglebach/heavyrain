public class Spike : Hazard {
    public override void ApplyEffect(Player player) {
        player.RemoveHealth();
    }
}
