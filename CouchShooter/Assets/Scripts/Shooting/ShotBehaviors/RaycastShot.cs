using UnityEngine;

/// <summary>
/// Instant-hit shot: raycasts from the muzzle along the aim direction out to the
/// gun's range. If it lands on a collider carrying an <see cref="InvokeEventOnHit"/>,
/// it delivers the gun's damage via <see cref="InvokeEventOnHit.Hit"/>. Slots into
/// any cadence via <see cref="AShotBehavior"/>.
/// </summary>
[CreateAssetMenu(fileName = "RaycastShot", menuName = "Scriptable Objects/ShotBehaviors/RaycastShot")]
public class RaycastShot : AShotBehavior
{
    [Header("Debug")]
    // When on, draws the shot in the Scene view (and the Game view with Gizmos
    // enabled): green to the hit point, red along the full range on a miss.
    [SerializeField] private bool drawDebugRay = false;
    [SerializeField] private float debugRayDuration = 1f;

    public override void Fire(Vector3 firePosition, Vector3 direction, AGun gun)
    {
        bool hasHit = Physics.Raycast(firePosition, direction, out RaycastHit hit, gun.GetRange());

        if (this.drawDebugRay)
        {
            Vector3 lineEnd = hasHit ? hit.point : firePosition + direction * gun.GetRange();
            Debug.DrawLine(firePosition, lineEnd, hasHit ? Color.green : Color.red, this.debugRayDuration);
        }

        if (!hasHit) return;
        if (!hit.collider.gameObject.TryGetComponent(out InvokeEventOnHit hittable)) return;
        hittable.Hit(new HitInfo(gun.GetDamagePerShot(), hit.point));
    }
}
