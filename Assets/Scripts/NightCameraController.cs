using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NightCameraController : MonoBehaviour
{
    // --- public --- //
    
    public GameObject monitorObject;
    
    // the up and down transforms for the monitor to move between
    public Transform transformEnabled;
    public Transform transformDisabled;
    
    // the state of the camera being up or down
    public bool isCameraOpen;
    
    // the speed at which the camera's open
    [Range(0, 1)] public float toggleDuration = 0.5f;
    
    // camera system positions
    [Header("CamSys Settings")]
    public List<Transform> cameraTransforms;

    public int cameraIndex;
    
    // --- private --- //

    private bool _animationLock;
    
    
    // --- functions --- //
    
    private IEnumerator AnimateCameraMonitor(bool newState)
    {
        _animationLock = true;
        
        // if the desired state is the same as the current, do nothing
        if (newState == isCameraOpen) yield break;
        
        var positionOrigin = monitorObject.transform.position;
        var rotationOrigin = monitorObject.transform.rotation;

        var transformTarget = newState ? transformEnabled : transformDisabled;
        var positionTarget = transformTarget.position;
        var rotationTarget = transformTarget.rotation;
        
        // start moving the monitor
        var elapsed = 0.0f;
        while (elapsed < toggleDuration)
        {
            var amt = Mathf.SmoothStep(0.0f, 1.0f, elapsed / toggleDuration);
            monitorObject.transform.position = Vector3.Lerp(positionOrigin, positionTarget, amt);
            monitorObject.transform.rotation = Quaternion.Slerp(rotationOrigin, rotationTarget, amt);
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        isCameraOpen = newState;
        _animationLock = false;
    }

    private void ToggleCameras()
    {
        var newCamState = !isCameraOpen;
        
        // if the cameras are already open, we want to close the UI immediately
        if  (isCameraOpen)
            isCameraOpen = newCamState;
        
        StartCoroutine(AnimateCameraMonitor(newCamState));
    }

    private int LoopInt(int value, int min, int max)
    {
        if (value < min) return max;
        if (value > max) return min;
        return value;
    }
    
    
    // --- events --- //
    
    void OnViewCameras(InputValue _)
    {
        if (_animationLock) return;
        ToggleCameras();
    }

    void OnChangeCamera(InputValue inputValue)
    {
        var value = Mathf.CeilToInt(inputValue.Get<float>());
        if (value == 0) return;
        
        //cameraIndex = Mathf.Clamp(cameraIndex + value, 0, cameraTransforms.Count - 1);
        cameraIndex = LoopInt(cameraIndex + value, 0, cameraTransforms.Count - 1);
    }
}
