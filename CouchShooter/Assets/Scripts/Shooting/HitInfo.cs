using UnityEngine;

/// <summary>
/// Immutable data for a single hit, passed from the shot into an
/// <see cref="InvokeEventOnHit"/> and on to whatever reacts to it. Starts with
/// the damage amount and hit point; more fields (direction, source) can be added
/// later if a reaction needs them.
/// </summary>
public struct HitInfo
{
    public readonly float damageAmount;
    public readonly Vector3 hitPoint;

    public HitInfo(float damageAmount, Vector3 hitPoint)
    {
        this.damageAmount = damageAmount;
        this.hitPoint = hitPoint;
    }
}
