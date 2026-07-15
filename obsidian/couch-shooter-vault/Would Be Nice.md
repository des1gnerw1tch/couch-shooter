# Would Be Nice

Ideas and polish we'd like to get to eventually. Pull items from here into
[[TODO]] when we decide to actually work on them.

## Shooting
- Spawn impact FX at the hit point when a shot lands.

## Tooling
- [ ] (Maybe) Add a `SessionStart` hook in `.claude/settings.json` that cats
      `Claude Instructions.md` into context every session, so Claude always reads
      the preferences deterministically rather than relying on the CLAUDE.md
      pointer instruction.

## Rendering
- [ ] Prevent the gun from clipping into walls. Approach to explore: render the
      viewmodel (gun/arms) with a **separate camera** on its own layer, then
      overlay that render on top of the main scene camera. The weapon camera
      draws the gun last (or clears depth) so geometry never intersects it.
      Details TBD — need to work out camera stacking, layers/culling masks, and
      FOV matching between the two cameras.