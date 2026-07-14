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

> This is the crux. Everything below is just *how far* we formalize this. We do
> NOT have to solve it all today.

## Open questions (for the user to drive)

- [ ] Do we even need auto + burst in v1, or start with semi-auto only and add
      the rest once one gun feels good?
- [ ] For "what a shot does," is hitscan-only fine for now, projectiles strictly
      later?
- [ ] How fancy do we want gun config? (numbers hardcoded per script vs. tweak
      in the Unity inspector vs. shared data assets) — pick the *simplest* that
      isn't annoying.
- [ ] What can take damage first? (dummy target that logs "hit" is enough to
      start)

## Decisions so far

- Hitscan first; keep the door open for projectiles later.
- Want semi / auto / burst eventually.
- Prefer NOT to cram every fire style into one giant script.

## Parking lot (ideas we're deliberately NOT doing yet)

- ScriptableObject weapon data assets
- Weapon switching (the Previous/Next input actions exist, save for later)
- Spread, recoil, reload timing, muzzle/impact FX
- Multiplayer per-player ownership details

## Next step

Decide the v1 scope from the open questions above, then pick the *smallest*
first thing to actually build (probably: one semi-auto hitscan gun that damages
a dummy target).
