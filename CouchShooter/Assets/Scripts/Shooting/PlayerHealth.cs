using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A reaction to being hit (not a target itself). Holds the hitboxes that feed
/// one health pool and subscribes to each one's hit event. For now it only logs
/// the hit; real health handling comes later.
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private List<InvokeEventOnHit> hitboxes = new List<InvokeEventOnHit>();

    private void OnEnable() => this.hitboxes.ForEach(hitbox => hitbox.GetOnHitEvent().AddListener(this.HandleHit));

    private void OnDisable() => this.hitboxes.ForEach(hitbox => hitbox.GetOnHitEvent().RemoveListener(this.HandleHit));

    // TODO (human): subtract from a real health pool, handle death, etc.
    private void HandleHit(HitInfo info) =>
        Debug.Log($"PlayerHealth hit: damage={info.damageAmount} at {info.hitPoint}");
}
