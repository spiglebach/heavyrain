public class Clock : FallingObject {
    public override void ApplyEffect(Player player) {
        base.ApplyEffect(player);
        player.AddWait();
    }
}
