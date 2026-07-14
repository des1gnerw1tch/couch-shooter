# C# Conventions

- Use expression-bodied members (`=>`) for one-line methods.
- Prefix all member variable/field references with `this.`.
- Be explicit with access modifiers — write `private` (etc.) in front of every
  field and method rather than relying on the implicit default.
- Spell it **`Behavior`** (US spelling), not `Behaviour`, in all our own type
  and member names.
- Expose stats/state as getter **methods** with `()` (e.g. `GetDamagePerShot()`),
  not properties.
