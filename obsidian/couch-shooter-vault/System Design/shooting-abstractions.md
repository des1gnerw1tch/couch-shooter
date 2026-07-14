# Shooting Abstractions

A shared scratchpad for designing the weapon/shooting system. **The user is in
the front seat** — this doc captures decisions as we make them, not a finished
plan. Nothing here is code yet.

## Goal (plain terms)

Support several kinds of guns without rewriting the shooting code each time.
Known wants:
- Fire styles: **semi-auto** (one shot per click), **auto** (hold to keep
  firing), **burst** (a fixed few shots per click).
- Start with **hitscan** (instant ray — the bullet hits the moment you click).
- Later add **projectiles** (grenades, rockets — a thing that flies through the
  air and can miss / arc).

## The one idea to get comfortable with

A gun really answers **two separate questions**, and they don't depend on each
other:

1. **WHEN do I fire?** → semi-auto / auto / burst
2. **WHAT does a shot do?** → hitscan ray / flying projectile

Because they're independent, any combo is valid (an auto hitscan SMG, a burst
grenade launcher, etc.). If we bake both into one thing, we get a mess. If we
keep them as two separate knobs, adding a new gun = pick a value for each knob.

## WIP design 

### AGun
Cadence lives in a class hierarchy; "what a shot does" is composed in.

```
AGun : MonoBehaviour (abstract)         cadence + gun state
  ├─ FireButtonPressed()                input events (map to OnAttack press/
  ├─ FireButtonReleased()               release via Send Messages)
  ├─ CanFire()  (shared gate)           ammo left? not reloading? fire-rate ok?
  ├─ Fire()                             spends ammo, then shotBehaviour.Fire(...)
  ├─ GetReloadTime()
  ├─ reload cancelling                  (TBD — see parking lot)
  ├─ private [SerializeField] stats:    fireRate, damagePerShot, reloadTime,
  │                                     fireSound (string),
  │                                     lookDownScopeTime (future)
  │   → exposed via public getters so the shot behaviour can read them
  └─ IShotBehaviour shotBehaviour       composed in
        ├─ HitscanShot
        └─ ProjectileShot (later)

  subclasses (the cadence):
    ASemiAutoGun    fire once on FireButtonPressed. NO Update().
    AFullyAutoGun   press sets held=true, release clears it. Own Update()
                    fires each frame CanFire() is true.
    ABurstGun       press starts a burst. Own Update() fires N shots spaced by
                    fireRate. Has a flag for whether holding the trigger repeats
                    the burst (fully-auto burst) or requires a re-press per
                    burst (semi burst).
```

### IShotBehaviour (the "what a shot does" knob)

Composed into `AGun`. One interface, one method:

```
interface IShotBehaviour
    void Fire(Vector3 firePosition, Vector3 direction, AGun gun)
```

- `firePosition` — where the shot starts (the muzzle).
- `direction` — where it's aimed.
- `gun` — the whole gun, so the behaviour can read stats via getters
  (`gun.DamagePerShot`, `gun.Range`, etc.) without duplicating them.

### Implementations

- **HitscanShot** (first): raycast from `firePosition` along `direction` out to
  the gun's range. If it hits something with health, deal `gun.DamagePerShot`.
  Spawn impact FX at the hit point.
- **ProjectileShot** (later): spawn a projectile prefab at `firePosition`
  heading in `direction`; the projectile handles its own travel + collision.
  Slots in beside HitscanShot with no change to any cadence class — a burst
  grenade launcher is just `ABurstGun` + `ProjectileShot`.

Anything that can be shot implements a small **IDamageable** (`TakeDamage(...)`);
the shot behaviour is what calls it. First target can just be a dummy that logs.
### Decisions locked in

- **Hitscan first**, projectiles later; keep the door open for them.
- **`CanFire()` is shared in `AGun`.** Same three checks for every gun: ammo
  left, not reloading, fire-rate cooldown elapsed.
- **No `Update()` in `AGun`.** Each cadence subclass that needs a heartbeat
  defines its own `Update()`; Unity calls it automatically. Semi-auto has none
  (fires purely on the press event), so it pays no per-frame cost.
- **`Fire()` vs the shot are cleanly separated.** Cadence code only touches the
  timing around firing; it never knows how a shot works.
- **Stats are private `[SerializeField]` on `AGun`** with public getters.

| Cadence       | Reacts to                       | Has Update? |
| ------------- | ------------------------------- | ----------- |
| ASemiAutoGun  | FireButtonPressed → fire once   | No          |
| AFullyAutoGun | press sets held, release clears | Yes         |
| ABurstGun     | press starts burst              | Yes         |



## Open questions / TBD

- [x] **v1 scope:** ship semi-auto only first and add auto + burst once one gun
      feels good, or build all three cadences up front?
	- [x] Let's add all three cadences up front
- [ ] **Burst repeat:** holding the trigger auto-repeats the burst, or requires
      a re-press? → a flag on `ABurstGun` would be good to support both of these; let's pick the default.
- [ ] **Reload cancelling** mechanism (see parking lot).
- [ ] **`IShotBehaviour` inspector-selectable per gun?** Unity needs
      `[SerializeReference]` for a polymorphic interface field — decide when we
      build it.

## Parking lot (ideas we're deliberately NOT doing yet)

- Weapon switching (the Previous/Next input actions exist, save for later)

## Next step

Decide v1 scope from the open questions above, then pick the *smallest* first
thing to actually build (probably: one semi-auto hitscan gun that damages a
dummy target).
