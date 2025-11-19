using UnityEngine;
using UnityEngine.InputSystem;

public class FreeroamPlayerController : MonoBehaviour
{
    // --- Public --- //
    
    // movement speed
    public float speed = 10;
    
    // the camera that the player should look around with
    public GameObject camera;
    
    // --- Private --- //

    private Rigidbody _rb;
    
    private Vector3 _moveDirection;
    private Quaternion _rotateDirection;
    private Quaternion _cameraOriginalRotation;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _cameraOriginalRotation = camera.transform.rotation;
    }

    void FixedUpdate()
    {
        // rotate the camera
        var mouseX = Input.GetAxis("Mouse X");
        var mouseY = Input.GetAxis("Mouse Y");
        
        
        // move the player
        var movement = _moveDirection * (speed * Time.deltaTime);
        _rb.AddForce(movement, ForceMode.VelocityChange);
    }

    void OnMove(InputValue movementValue)
    {
        var moveInput = movementValue.Get<Vector2>();
        _moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
    }
}
