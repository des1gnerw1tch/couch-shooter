using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The seam between the shooting system and anything that can be shot. Put this
/// on a collider object; a shot that hits that collider calls <see cref="Hit"/>,
/// which raises the event. Reactions (health, sound, break, ...) live anywhere
/// and register via <c>GetOnHitEvent().AddListener(...)</c>; this component never
/// knows what happens next.
/// </summary>
public class InvokeEventOnHit : MonoBehaviour
{
    private UnityEvent<HitInfo> onHit = new UnityEvent<HitInfo>();

    /// <summary>The event reactions subscribe to.</summary>
    public UnityEvent<HitInfo> GetOnHitEvent() => this.onHit;

    /// <summary>Called by a shot that landed on this object.</summary>
    public void Hit(HitInfo info) => this.onHit.Invoke(info);
}
