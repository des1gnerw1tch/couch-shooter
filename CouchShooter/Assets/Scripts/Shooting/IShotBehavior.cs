using UnityEngine;

/// <summary>
/// The "what does a shot do?" knob, composed into an <see cref="AGun"/>.
/// A cadence class decides WHEN to fire; the shot behavior decides WHAT a
/// single shot does (hitscan ray, flying projectile, ...). Any cadence works
/// with any shot behavior.
/// </summary>
public interface IShotBehavior
{
    /// <summary>
    /// Resolve a single shot.
    /// </summary>
    /// <param name="firePosition">Where the shot starts (the muzzle).</param>
    /// <param name="direction">Where it is aimed.</param>
    /// <param name="gun">The firing gun, so the behavior can read stats
    /// (damage, range, ...) via its public getters.</param>
    void Fire(Vector3 firePosition, Vector3 direction, AGun gun);
}
