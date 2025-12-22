using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPMovementController : MonoBehaviour
{
    // --- public --- //
    
    public List<Transform> cameraTransforms;
    
    public int officeIndex = 1;
    public int officeDeskIndex = 1;
    
    // The speed that the player moves in the office
    [Range(0, 1)] public float moveDuration = 0.5f;
    
    
    // --- private --- //

    private bool _animationLock;
    private NPCameraController _cameraController;
    
    
    // --- computed --- //
    
    public bool IsAtDesk => officeIndex == officeDeskIndex;
    
    
    // --- functions --- //

    private IEnumerator MoveOfficeCamera()
    {
        _animationLock = true;
        
        // get the origin and target transforms for the player
        var originPosition = transform.position;
        var originRotation = transform.rotation;
        
        var targetPosition = cameraTransforms[officeIndex].position;
        var targetRotation = cameraTransforms[officeIndex].rotation;
        
        
        // begin moving the camera
        var elapsed = 0.0f;
        while (elapsed < moveDuration)
        {
            var amt = Mathf.SmoothStep(0.0f, 1.0f, elapsed / moveDuration);
            transform.position = Vector3.Lerp(originPosition, targetPosition, amt);
            transform.rotation = Quaternion.Slerp(originRotation, targetRotation, amt);
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        transform.position = targetPosition;
        transform.rotation = targetRotation;
        
        _animationLock = false;
    }
    
    
    // --- events --- //

    void Start()
    {
        _cameraController = GetComponent<NPCameraController>();
    }

    void OnMove(InputValue inputValue)
    {
        if (_cameraController.CameraState) return; // dont move if cams are open
        if (_animationLock) return;
        
        var value = inputValue.Get<Vector2>();
        if (value == Vector2.zero) return;
        
        // ensure the new index is within range
        var xInt = Mathf.CeilToInt(value.x);
        officeIndex = Mathf.Clamp(officeIndex + xInt, 0, cameraTransforms.Count - 1);
            
        StartCoroutine(MoveOfficeCamera());
        
        // if we arnt at the desk, disable cameras
        _cameraController.camerasEnabled = IsAtDesk;
    }
}