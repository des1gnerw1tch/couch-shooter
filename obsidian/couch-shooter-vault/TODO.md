# TODO

## Gameplay
- [ ] Shooting scripts (fire weapon, raycast/projectile, damage, fire rate, ammo).
      Design it to be **general** so it supports multiple different gun types
      (e.g. pistol, shotgun, automatic rifle) — shared base behavior with
      per-weapon config for fire rate, damage, spread, ammo, projectile type, etc.

## Rendering
- [ ] Prevent the gun from clipping into walls. Approach to explore: render the
      viewmodel (gun/arms) with a **separate camera** on its own layer, then
      overlay that render on top of the main scene camera. The weapon camera
      draws the gun last (or clears depth) so geometry never intersects it.
      Details TBD — need to work out camera stacking, layers/culling masks, and
      FOV matching between the two cameras.

## Tooling
- [ ] (Maybe) Add a `SessionStart` hook in `.claude/settings.json` that cats
      `Claude Instructions.md` into context every session, so Claude always reads
      the preferences deterministically rather than relying on the CLAUDE.md
      pointer instruction.
