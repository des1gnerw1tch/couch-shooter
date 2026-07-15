using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Base class for every gun. Owns gun state and the shared firing gate; each
/// concrete subclass supplies only its cadence (WHEN to fire). WHAT a shot does
/// is composed in via <see cref="IShotBehavior"/>, so any cadence pairs with
/// any shot behavior.
///
/// Attach a PlayerInput (Behavior = "Broadcast Messages") on the same object; its
/// OnAttack message is routed to <see cref="FireButtonPressed"/> /
/// <see cref="FireButtonReleased"/>.
/// </summary>
public abstract class AGun : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float fireRate = 8f;          // shots per second
    [SerializeField] private float damagePerShot = 10f;
    [SerializeField] private float range = 100f;
    [SerializeField] private float reloadTime = 1.5f;
    [SerializeField] private int magazineSize = 30;
    [SerializeField] private string fireSound = "";

    [Header("Shot")]
    // The muzzle: its position is where a shot starts and its forward (+Z) is
    // the aim direction. Serialized so it is NOT assumed to be the gun's own
    // transform.
    [SerializeField] private Transform muzzle;
    [SerializeReference] private IShotBehavior shotBehavior = new MockShotBehavior();

    private int ammoInMagazine;
    private float lastFireTime = -Mathf.Infinity;
    private bool isReloading;

    // Stats exposed for the shot behavior to read without duplicating them.
    public float GetDamagePerShot() => this.damagePerShot;
    public float GetRange() => this.range;
    public float GetFireRate() => this.fireRate;
    public string GetFireSound() => this.fireSound;
    public float GetReloadTime() => this.reloadTime;

    /// <summary>Seconds between shots implied by the fire rate.</summary>
    protected float GetFireInterval() => 1f / this.fireRate;

    protected virtual void Awake() => this.ammoInMagazine = this.magazineSize;

    // Input routing (PlayerInput "Broadcast Messages"). OnAttack carries both press
    // and release in one message; isPressed distinguishes them.
    public void OnAttack(InputValue value)
    {
        if (value.isPressed) this.FireButtonPressed();
        else this.FireButtonReleased();
    }

    /// <summary>Trigger pulled. Cadence subclasses react here.</summary>
    protected abstract void FireButtonPressed();

    /// <summary>Trigger released. No-op unless a cadence needs it.</summary>
    protected virtual void FireButtonReleased() { }

    /// <summary>Shared gate: ammo left, not reloading, fire-rate cooldown up.</summary>
    protected bool CanFire() =>
        this.ammoInMagazine > 0
        && !this.isReloading
        && Time.time - this.lastFireTime >= this.GetFireInterval();

    /// <summary>Spend ammo, stamp the cooldown, then resolve the shot.</summary>
    protected void Fire()
    {
        this.ammoInMagazine--;
        this.lastFireTime = Time.time;
        if (fireSound != "")
        {
            AudioManager.Instance.Play(fireSound);
        }
        this.shotBehavior.Fire(this.muzzle.position, this.muzzle.forward, this);
    }
}
