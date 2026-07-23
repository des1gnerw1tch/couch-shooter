# TODO

## Gameplay

### Shooting
See [[shooting-abstractions]] for the design.
- [x] Shooting abstraction scripts (fire weapon, raycast/projectile, damage,
      fire rate, ammo). Design it to be **general** so it supports multiple
      different gun types (e.g. pistol, shotgun, automatic rifle) — shared base
      behavior with per-weapon config for fire rate, damage, spread, ammo,
      projectile type, etc.
- [x] Add audio to guns (fire sounds via AudioManager).
- [x] Create the shot-target abstraction (what a shot can hit and how it deals
      damage) — see [[shot-target-abstraction]]. Do this before implementing type of Shots, since both depend on it.
- [x] Implement `RaycastShot` — instant raycast from the muzzle that deals
      damage to whatever it hits.
- [ ] Implement `ProjectileShot` — spawn a projectile that flies from the muzzle
      and handles its own travel + collision. Slots in beside `RaycastShot` with
      no change to any cadence class.



