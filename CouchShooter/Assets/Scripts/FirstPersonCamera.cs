using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// First-person mouse/stick look using the Look action. Yaw (horizontal)
/// rotates the whole player body so movement follows the look direction; pitch
/// (vertical) rotates only the camera so the body stays upright.
/// </summary>
public class FirstPersonCamera : MonoBehaviour
{
    [SerializeField] Transform cameraTransform;
    [SerializeField] float sensitivity = 0.1f;
    [SerializeField] float startingPitch = 0f;
    [SerializeField] float minPitch = -85f;
    [SerializeField] float maxPitch = 85f;

    Vector2 lookInput;
    float pitch;

    void Awake() => this.pitch = this.startingPitch;

    void OnEnable() => Cursor.lockState = CursorLockMode.Locked;

    public void OnLook(InputValue value) => this.lookInput = value.Get<Vector2>();

    void Update()
    {
        // Yaw rotates the body around the world up axis.
        this.transform.Rotate(Vector3.up, this.lookInput.x * this.sensitivity, Space.World);

        // Pitch rotates only the camera, clamped so you can't flip over.
        this.pitch = Mathf.Clamp(this.pitch - this.lookInput.y * this.sensitivity, this.minPitch, this.maxPitch);
        this.cameraTransform.localRotation = Quaternion.Euler(this.pitch, 0f, 0f);
    }
}
