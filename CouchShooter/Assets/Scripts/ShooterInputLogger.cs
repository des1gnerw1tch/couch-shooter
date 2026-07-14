using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Logs every Player-map input action for debugging. Attach to the player
/// prefab alongside a PlayerInput component whose Behavior is set to
/// "Send Messages"; the On&lt;Action&gt; method names are matched automatically.
/// </summary>
public class ShooterInputLogger : MonoBehaviour
{
    // Value actions (Vector2).
    public void OnMove(InputValue value) => Debug.Log($"Move: {value.Get<Vector2>()}");
    public void OnLook(InputValue value) => Debug.Log($"Look: {value.Get<Vector2>()}");

    // Button actions (isPressed is true on press, false on release).
    public void OnAttack(InputValue value) => Debug.Log($"Attack: {value.isPressed}");
    public void OnInteract(InputValue value) => Debug.Log($"Interact: {value.isPressed}");
    public void OnCrouch(InputValue value) => Debug.Log($"Crouch: {value.isPressed}");
    public void OnJump(InputValue value) => Debug.Log($"Jump: {value.isPressed}");
    public void OnPrevious(InputValue value) => Debug.Log($"Previous: {value.isPressed}");
    public void OnNext(InputValue value) => Debug.Log($"Next: {value.isPressed}");
    public void OnSprint(InputValue value) => Debug.Log($"Sprint: {value.isPressed}");
}
