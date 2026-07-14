/// <summary>
/// Full-auto cadence: hold the trigger to keep firing. The press marks the
/// trigger held and the release clears it; <see cref="Update"/> fires on every
/// frame the shared gate allows while held. The fire-rate cooldown in
/// <see cref="AGun.CanFire"/> paces the shots.
/// </summary>
public class FullAutoGun : AGun
{
    private bool triggerHeld;

    protected override void FireButtonPressed() => this.triggerHeld = true;

    protected override void FireButtonReleased() => this.triggerHeld = false;

    private void Update()
    {
        if (this.triggerHeld && this.CanFire()) this.Fire();
    }
}
