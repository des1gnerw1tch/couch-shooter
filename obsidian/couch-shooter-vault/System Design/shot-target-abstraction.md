# Shot-Target Abstraction

A shared scratchpad for designing **what a shot can hit and hurt**. Companion to
[[shooting-abstractions]], which left this as TBD: *"How a shot target is
defined (what 'something with health' means) is TBD — we'll brainstorm that when
we get to it."* **The user is in the front seat** — this captures decisions as we
make them, not a finished plan. Nothing here is code yet.

## Where this fits

The cadence layer (WHEN to fire) and `IShotBehavior` (WHAT a shot does) are
built and working against `MockShotBehavior`. The next real behavior is
`HitscanShot`: raycast from the muzzle out to `gun.GetRange()`, and deal
`gun.GetDamagePerShot()` to whatever it hits.

`HitscanShot` needs one thing this doc has to define: **given a raycast hit,
how do we know if it's a valid target, and how do we tell it "take N damage"?**

## The question to answer

When a ray (or later, a projectile) hits a collider, we need to answer:

1. **Is this hittable?** — a wall vs. an enemy vs. a friendly vs. a prop.
2. **How do we apply damage to it?** — what does the shot call?

This is the seam between the shooting system and everything that can be shot.

## Open questions / TBD

- [ ] What is the contract a target exposes? (interface? component? something else)
- [ ] How does a hitscan ray get from a `RaycastHit` to that contract?
- [ ] What info does a target need about a hit? (amount, direction, source, hit point?)
- [ ] Layers / masks — how do we avoid raycasting against things that can't be shot?
- [ ] What happens on a non-target hit (wall, ground)? (nothing? impact FX later?)

## Decisions locked in

_(none yet)_

## Parking lot (deliberately not now)

_(none yet)_
