/// <summary>
/// Semi-auto cadence: one shot per trigger pull. Fires purely on the press
/// event, so it needs no <c>Update()</c> and pays no per-frame cost.
/// </summary>
public class SemiAutoGun : AGun
{
    protected override void FireButtonPressed()
    {
        if (this.CanFire()) this.Fire();
    }
}
