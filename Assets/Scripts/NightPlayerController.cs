using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NightPlayerController : MonoBehaviour
{
    // --- Public --- //
    
    // the transforms of each position for the office camera to move to
    [Header("The Office")]
    public List<Transform> officeCameraTransforms;
    
    // the player's current position in the office
    [Range(0, 2)] public int currentOfficeIndex = 1;
    
    // The speed that the player moves in the office
    [Range(0, 1)] public float officeMoveDuration = 0.5f;
    
    // the flashlight
    [Header("The Flashlight")]
    public Light flashlight;
    
    // the camera view
    [Header("The Cameras")]
    public GameObject cameraMonitor;
    
    // the up and down transforms for the monitor to move between
    public Transform cameraTransformEnabled;
    public Transform cameraTransformDisabled;
    
    // the state of the camera being up or down
    public bool isCameraOpen;
    
    // the speed at which the camera's open
    [Range(0, 1)] public float cameraToggleDuration = 0.5f;
    
    // --- Office Movement --- //

    private IEnumerator MoveOfficeCamera()
    {
        // get the origin and target transforms for the player
        var positionOrigin = transform.position;
        var rotationOrigin = transform.rotation;
        
        var positionTarget = officeCameraTransforms[currentOfficeIndex].position;
        var rotationTarget = officeCameraTransforms[currentOfficeIndex].rotation;
        
        // begin moving te camera
        var elapsed = 0.0f;
        while (elapsed < officeMoveDuration)
        {
            var amt = Mathf.SmoothStep(0.0f, 1.0f, elapsed / officeMoveDuration);
            transform.position = Vector3.Lerp(positionOrigin, positionTarget, amt);
            transform.rotation = Quaternion.Slerp(rotationOrigin, rotationTarget, amt);
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        transform.position = positionTarget;
        transform.rotation = rotationTarget;
    }

    void OnMove(InputValue inputValue)
    {
        var value = inputValue.Get<Vector2>();
        if (value == Vector2.zero) return;
        
        var xInt = Mathf.CeilToInt(value.x);
        currentOfficeIndex = Mathf.Clamp(currentOfficeIndex + xInt, 0, officeCameraTransforms.Count - 1);
            
        StartCoroutine(MoveOfficeCamera());
    }
    
    // --- The Flashlight --- //
    
    void OnFlash(InputValue _)
    {
        flashlight.enabled = !flashlight.enabled;
    }
    
    // --- Cameras --- //

    private IEnumerator ToggleCameras(bool newState)
    {
        // if the desired state is the same as the current, do nothing
        if (newState == isCameraOpen) yield break;
        
        // decide the origin and target transforms
        var positionOrigin = cameraMonitor.transform.position;
        var rotationOrigin = cameraMonitor.transform.rotation;
        
        // the target should be the oppisite o
        var transformTarget = newState ? cameraTransformEnabled : cameraTransformDisabled;
        var positionTarget = transformTarget.position;
        var rotationTarget = transformTarget.rotation;
        
        // start moving the monitor
        var elapsed = 0.0f;
        while (elapsed < cameraToggleDuration)
        {
            var amt = Mathf.SmoothStep(0.0f, 1.0f, elapsed / cameraToggleDuration);
            cameraMonitor.transform.position = Vector3.Lerp(positionOrigin, positionTarget, amt);
            cameraMonitor.transform.rotation = Quaternion.Slerp(rotationOrigin, rotationTarget, amt);
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        // once everything has moved, declare that the camera has changed
        isCameraOpen = newState;
    }

    void OnViewCameras(InputValue _)
    {
        StartCoroutine(ToggleCameras(!isCameraOpen));
    }
}