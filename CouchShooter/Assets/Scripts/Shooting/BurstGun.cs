using UnityEngine;

/// <summary>
/// Burst cadence: one trigger pull fires a fixed number of shots. The burst
/// runs in <see cref="Update"/>, one shot per frame the shared gate allows, so
/// the fire-rate cooldown in <see cref="AGun.CanFire"/> spaces the shots
/// *within* a burst. A separate <see cref="timeBetweenBursts"/> gap must then
/// elapse before the next burst can start, so bursts stay distinct rather than
/// blurring into full-auto.
///
/// <see cref="repeatWhileHeld"/> chooses between the two burst styles: off
/// (default) requires a fresh press per burst (semi burst); on repeats the
/// burst for as long as the trigger is held (fully-auto burst).
///
/// TODO (human): revisit this burst state machine — the interplay between
/// FireButtonPressed and Update starting/restarting a burst feels fragile and
/// deserves a careful second look. See the note on Update below.
/// </summary>
public class BurstGun : AGun
{
    [Header("Burst")]
    [SerializeField] private int burstCount = 3;
    // Recovery gap after a burst ends before another may begin. Distinct from
    // fireRate, which only spaces the shots inside a single burst.
    [SerializeField] private float timeBetweenBursts = 0.3f;
    [SerializeField] private bool repeatWhileHeld = false;

    private int shotsRemainingInBurst;
    private bool triggerHeld;
    private float lastBurstEndTime = -Mathf.Infinity;

    protected override void FireButtonPressed()
    {
        this.triggerHeld = true;
        // A press only kicks off a burst when we are idle AND the between-burst
        // recovery has elapsed; presses during an in-flight burst are ignored.
        if (this.CanStartBurst()) this.StartBurst();
    }

    protected override void FireButtonReleased() => this.triggerHeld = false;

    private bool CanStartBurst() =>
        this.shotsRemainingInBurst == 0
        && Time.time - this.lastBurstEndTime >= this.timeBetweenBursts;

    private void StartBurst() => this.shotsRemainingInBurst = this.burstCount;

    private void Update()
    {
        // Claude: FireButtonPressed and this Update never run concurrently —
        // Unity dispatches input messages and Update on the same (main) thread,
        // so a burst can't be started twice at once. StartBurst is also an
        // assignment (not additive), so even a same-frame press + restart just
        // (re)sets burstCount rather than stacking.
        // TODO (human): review Claude's reasoning above (esp. the
        // repeatWhileHeld restart path) before trusting it.
        if (this.shotsRemainingInBurst > 0)
        {
            if (this.CanFire())
            {
                this.Fire();
                this.shotsRemainingInBurst--;
                // Stamp the end time on the burst's final shot so the
                // between-burst gap is measured from when it actually ended.
                if (this.shotsRemainingInBurst == 0) this.lastBurstEndTime = Time.time;
            }
        }
        else if (this.repeatWhileHeld && this.triggerHeld && this.CanStartBurst())
        {
            this.StartBurst();
        }
    }
}
