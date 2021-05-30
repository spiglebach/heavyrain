public class Clock : FallingObject {
    public override void ApplyEffect(Player player) {
        player.AddSkip();
    }
}
