using System;
using UnityEngine;

/// <summary>
/// Placeholder <see cref="IShotBehavior"/> that only logs. Lets us test the
/// cadence classes (semi-auto / auto / burst) before real hitscan or projectile
/// shots exist. <see cref="Serializable"/> so it can be assigned to a gun's
/// <c>[SerializeReference]</c> shot-behavior field in the inspector.
/// </summary>
[Serializable]
public class MockShotBehavior : IShotBehavior
{
    // Time.time of the previous shot, for logging the gap between shots.
    private float lastShotTime = -Mathf.Infinity;

    public void Fire(Vector3 firePosition, Vector3 direction, AGun gun)
    {
        float sinceLastShot = Time.time - this.lastShotTime;
        this.lastShotTime = Time.time;
        Debug.Log($"Fired! pos={firePosition} dir={direction} damage={gun.GetDamagePerShot()} (since last shot: {sinceLastShot:F3}s)");
    }
}
