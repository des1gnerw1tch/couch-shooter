using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Moves the player using the Move action (WASD / left stick), relative to the
/// direction the body is currently facing.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 6f;

    Vector2 moveInput;

    public void OnMove(InputValue value) => this.moveInput = value.Get<Vector2>();

    void Update()
    {
        // Move relative to where the body is currently facing.
        Vector3 move = this.transform.right * this.moveInput.x + this.transform.forward * this.moveInput.y;
        this.transform.position += move * this.moveSpeed * Time.deltaTime;
    }
}
