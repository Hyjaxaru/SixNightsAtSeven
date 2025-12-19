using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class NightCameraController : MonoBehaviour
{
    // --- public --- //
    
    // whether cameras are allowed or not (used by NightPlayerController)
    public bool camerasEnabled;
    
    [Header("Camera Monitor")]
    public GameObject monitorObject;
    
    // the up and down transforms for the monitor to move between
    public Transform transformEnabled;
    public Transform transformDisabled;
    
    // the speed at which the camera's open
    [Range(0, 1)] public float toggleDuration = 0.5f;
    
    // camera system positions
    [Header("CamSys Settings")]
    public GameObject camSysCamera;
    
    // all the transforms for camera positions
    public List<Transform> cameraTransforms;
    
    // the current index in the camera position transform list
    public int cameraIndex;
    
    public TextMeshPro cameraIndexText;

    public bool CameraState
    {
        get => _camSysCameraComp.enabled;
        set => _camSysCameraComp.enabled = value;
    }
    
    
    // --- private --- //

    private bool _animationLock;
    private Camera _camSysCameraComp;
    private FlashlightController _flashlight;
    
    
    // --- functions --- //
    
    private IEnumerator AnimateCameraMonitor(bool newState)
    {
        _animationLock = true;
        
        // if the desired state is the same as the current, do nothing
        if (newState == CameraState) yield break;
        
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
        
        CameraState = newState;
        _animationLock = false;
    }

    private int LoopInt(int value, int min, int max)
    {
        if (value < min) return max;
        if (value > max) return min;
        return value;
    }

    private void SetCameraTransform(Transform t)
    {
        camSysCamera.transform.position = t.position;
        camSysCamera.transform.rotation = t.rotation;
    }

    private void SetCameraText()
    {
        cameraIndexText.text = "Camera " + (cameraIndex + 1);
    }
    
    
    // --- events --- //

    void Start()
    {
        _camSysCameraComp = camSysCamera.GetComponent<Camera>();
        _flashlight = GetComponent<FlashlightController>();

        _camSysCameraComp.enabled = false;
        SetCameraTransform(cameraTransforms[0]);
        SetCameraText();
    }
    
    void OnViewCameras(InputValue _)
    {
        if (!camerasEnabled) return;
        if (_animationLock) return;
        
        StartCoroutine(AnimateCameraMonitor(!CameraState));
        
        // if the cameras are already on, we want to disable them before the animation
        if (CameraState) CameraState = false;

        _flashlight.enabled = false;
    }

    void OnMove(InputValue inputValue)
    {
        if (!CameraState) return; // don't do anything if cams aren't open
        
        var value = inputValue.Get<Vector2>();
        if (value == Vector2.zero) return;
        var xInt = Mathf.CeilToInt(value.x);
        
        //cameraIndex = Mathf.Clamp(cameraIndex + value, 0, cameraTransforms.Count - 1);
        cameraIndex = LoopInt(cameraIndex + xInt, 0, cameraTransforms.Count - 1);
        
        SetCameraTransform(cameraTransforms[cameraIndex]);
        SetCameraText();
    }
}
