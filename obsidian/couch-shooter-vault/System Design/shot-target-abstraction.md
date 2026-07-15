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
2. **What happens when it's hit?** — damage, sound, break, ...

This is the seam between the shooting system and everything that can be shot.

## The design

Two roles, cleanly separated: **one component the shot pokes**, and **any number
of reactions that consume its event**. There is no interface — every hittable is
the same event-raising component, so the variety lives entirely in the consumers.

### `InvokeEventOnHit` — the seam

The one component the shot talks to. It holds a **private `UnityEvent<HitInfo>`**
exposed via a getter, and raises it when hit:

```csharp
public class InvokeEventOnHit : MonoBehaviour
{
    [SerializeField] private UnityEvent<HitInfo> onHit;

    public UnityEvent<HitInfo> GetOnHit() => this.onHit;   // consumers AddListener to this

    public void Hit(HitInfo info) => this.onHit.Invoke(info);
}
```

The shot resolves a `RaycastHit` to an `InvokeEventOnHit` component
(`TryGetComponent`) and calls `Hit(...)`. It never knows what the target *is* or
what happens next — that's all in the listeners. Making this component concrete
(rather than an interface) is deliberate: everything shootable goes through the
same event, no side-door implementations.

### `HitInfo` — what a hit carries

A small struct passed into `Hit`. Named `HitInfo` (not `DamageInfo`) because it
may carry more than damage over time. Starts with `damageAmount`; other fields
(hit point, direction, source) are cheap to carry and pay off later (knockback,
hit markers, "killed by" attribution).

### The reactions consume the event

The collider object hosts an `InvokeEventOnHit`. The reactions that respond to it
**do not have to live on that same GameObject** — each just holds a serialized
reference to the `InvokeEventOnHit`(s) it cares about and `AddListener`s to their
event. Reactions are named by what they do:

- **`PlayerHealth`** — subtracts `HitInfo.damageAmount` from a health pool.
- **`MakeSoundOnHit`** — plays a sound.
- **`BreakOnHit`** — destroys / shatters the object.
- (future) spawn-FX-on-hit, etc.

A barrel reacts with sound + break and no health; an enemy reacts with
`PlayerHealth`. Adding a new reaction never touches shooting code. Making the
collider object a **prefab** lets us drop it in and tune the collider per object.

### `PlayerHealth` — a reaction, not a target

`PlayerHealth` is a `MonoBehaviour` that holds a **serialized list of
`InvokeEventOnHit`** (all the hitboxes that feed one health pool — body, limbs,
..., wherever they live), `AddListener`s to each in `OnEnable`, and subtracts
`HitInfo.damageAmount`. It reacts to hits; it is not itself the thing the shot
calls.

```csharp
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private List<InvokeEventOnHit> hitboxes;

    private void OnEnable()  => this.hitboxes.ForEach(h => h.GetOnHit().AddListener(this.HandleHit));
    private void OnDisable() => this.hitboxes.ForEach(h => h.GetOnHit().RemoveListener(this.HandleHit));

    private void HandleHit(HitInfo info) => /* subtract info.damageAmount */;
}
```

Always `RemoveListener` in `OnDisable` to avoid dangling listeners.

## Wiring direction

Reactions (`PlayerHealth`, `MakeSoundOnHit`, `BreakOnHit`, ...) **`AddListener` to**
the `InvokeEventOnHit` event. The reaction reaches *toward* the hitbox, not the
other way around — so a barrel can have sound + break with no health at all, and
one entity's many hitboxes can all feed a single `PlayerHealth`.

## Walls still need a layer mask

Geometry isn't hittable but must still **stop the ray** so you can't shoot
through a wall into an enemy behind it. So the raycast uses a broad `LayerMask`
(world + hitboxes); only the colliders with an `InvokeEventOnHit` react.

## Decisions locked in

- **No interface.** `InvokeEventOnHit` (a concrete `MonoBehaviour`) is the seam
  the shot calls — one implementation ever, so an `IHittable` bought nothing.
- **`InvokeEventOnHit`** exposes `void Hit(HitInfo info)` and a **private
  `UnityEvent<HitInfo>` behind a getter** (`GetOnHit()`); reactions `AddListener`
  to it in code (`OnEnable`) and `RemoveListener` in `OnDisable`.
- **`UnityEvent` over `event Action`** — consumers register themselves on the
  source, keeping wiring `consumer → source`; keeps the option of inspector-wiring.
- **`HitInfo` struct** (not `DamageInfo`) — starts with `damageAmount`, room to grow.
- **Reactions named by role** (`PlayerHealth`, `MakeSoundOnHit`, `BreakOnHit`,
  ...), one per consequence — *not* one "Hitbox" class. They **need not live on
  the collider object** — each holds a serialized reference to its hitbox(es).
- **`PlayerHealth` holds a list of hitboxes and subscribes** — it is a reaction,
  not the target.
- **Raycast uses a `LayerMask`** so walls block the ray; only colliders with an
  `InvokeEventOnHit` react.

## Parking lot (deliberately not now)

- Headshot / weak-point hitboxes — `PlayerHealth` keeps a separate head-hitbox
  list and applies a multiplier itself (hitbox stays dumb). Tracked in
  [[Would Be Nice]].
- Impact FX / decals on non-hittable geometry. Tracked in [[Would Be Nice]].
