using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Logs every Player-map input action for debugging. Attach to the player
/// prefab alongside a PlayerInput component whose Behavior is set to
/// "Send Messages"; the On&lt;Action&gt; method names are matched automatically.
/// Toggle <see cref="logEnabled"/> off to silence it without removing the component.
/// </summary>
public class ShooterInputLogger : MonoBehaviour
{
    [SerializeField] private bool logEnabled = true;

    // Value actions (Vector2).
    public void OnMove(InputValue value) => this.Log($"Move: {value.Get<Vector2>()}");
    public void OnLook(InputValue value) => this.Log($"Look: {value.Get<Vector2>()}");

    // Button actions (isPressed is true on press, false on release).
    public void OnAttack(InputValue value) => this.Log($"Attack: {value.isPressed}");
    public void OnInteract(InputValue value) => this.Log($"Interact: {value.isPressed}");
    public void OnCrouch(InputValue value) => this.Log($"Crouch: {value.isPressed}");
    public void OnJump(InputValue value) => this.Log($"Jump: {value.isPressed}");
    public void OnPrevious(InputValue value) => this.Log($"Previous: {value.isPressed}");
    public void OnNext(InputValue value) => this.Log($"Next: {value.isPressed}");
    public void OnSprint(InputValue value) => this.Log($"Sprint: {value.isPressed}");

    private void Log(string message)
    {
        if (this.logEnabled) Debug.Log(message);
    }
}
