using UnityEngine;

/// <summary>
/// The "what does a shot do?" knob, composed into an <see cref="AGun"/>.
/// A cadence class (<see cref="SemiAutoGun"/>, <see cref="FullAutoGun"/>,
/// <see cref="BurstGun"/>) decides WHEN to fire; the shot behavior decides WHAT a
/// single shot does (raycast, flying projectile, ...). Any cadence works with
/// any shot behavior.
///
/// A <see cref="ScriptableObject"/>: each shot is a reusable asset dropped into
/// a gun's shot field.
/// </summary>
public abstract class AShotBehavior: ScriptableObject
{
    /// <summary>
    /// Resolve a single shot.
    /// </summary>
    /// <param name="firePosition">Where the shot starts (the muzzle).</param>
    /// <param name="direction">Where it is aimed.</param>
    /// <param name="gun">The firing gun, so the behavior can read stats
    /// (damage, range, ...) via its public getters.</param>
    public abstract void Fire(Vector3 firePosition, Vector3 direction, AGun gun);
}
