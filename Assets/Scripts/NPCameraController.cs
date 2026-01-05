using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPCameraController : MonoBehaviour
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
    
    // ui
    [Header("UI")]
    public TextMeshPro cameraIndexText;
    [Space]
    public TextMeshPro cameraTimeText;
    public TextMeshPro officeTimeText;
    [Space]
    public TextMeshPro cameraPowerText;
    public TextMeshPro officePowerText;
    
    // audio
    [Header("Audio")]
    public AudioSource switchCameraAudioSource;
    public AudioSource cameraToggleAudioSource;
    
    
    // --- private --- //

    private bool _animationLock;
    private Camera _camSysCameraComp;
    private NPFlashlightController _flashlight;
    
    
    // --- computed --- //
    
    public bool CameraState
    {
        get => _camSysCameraComp.enabled;
        set => _camSysCameraComp.enabled = value;
    }
    
    
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
        
        // audio
        if (newState) cameraToggleAudioSource.Play();
        
        CameraState = newState;
        _animationLock = false;
    }

    public IEnumerator SnapCameraMonitor(bool newState)
    {
        _animationLock = true;
        
        // if the desired state is the same as the current, do nothing
        if (newState == CameraState) yield break;
        
        // don't animate, just move
        var transformTarget = newState ? transformEnabled : transformDisabled;
        monitorObject.transform.position = transformTarget.position;
        monitorObject.transform.rotation = transformTarget.rotation;
        
        // audio
        if (newState) cameraToggleAudioSource.Play();
        
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

    private void SetCameraIndexText()
    {
        cameraIndexText.text = "Camera " + (cameraIndex + 1);
    }

    public void SetCurrentTimeText()
    {
        var text = GameManager.Instance.HourDisplay + " AM";
        cameraTimeText.text = text;
        officeTimeText.text = text;
    }

    public void SetPowerUsageText()
    {
        var power = GameManager.Instance.nightPower;
        var usage = GameManager.Instance.CurrentPowerUsage;
        
        var usageString = "";
        for (var i = 0; i < usage; i++)
        {
            usageString += "█ ";
        }

        var text = power / 10 + "%\nUsage: " + usageString;
        
        cameraPowerText.text = text;
        officePowerText.text = text;
    }

    public void ToggleOfficeUI(bool state)
    {
        officePowerText.enabled = state;
        officeTimeText.enabled = state;
    }
    
    // --- events --- //

    void Start()
    {
        _camSysCameraComp = camSysCamera.GetComponent<Camera>();
        _flashlight = GetComponent<NPFlashlightController>();

        _camSysCameraComp.enabled = false;
        SetCameraTransform(cameraTransforms[0]);
        SetCameraIndexText();
        SetCurrentTimeText();
        SetPowerUsageText();
    }
    
    void OnViewCameras(InputValue _)
    {
        if (GameManager.Instance.isPlayerDead) return;
        if (!camerasEnabled) return;
        if (_animationLock) return;
        
        StartCoroutine(AnimateCameraMonitor(!CameraState));
        
        // if the cameras are already on, we want to disable them before the animation
        if (CameraState)
        {
            CameraState = false;
            cameraToggleAudioSource.Play();
        }

        _flashlight.enabled = false;
    }

    void OnMove(InputValue inputValue)
    {
        if (GameManager.Instance.isPlayerDead) return;
        if (!CameraState) return; // don't do anything if cams aren't open
        
        var value = inputValue.Get<Vector2>();
        if (value == Vector2.zero) return;
        var xInt = Mathf.CeilToInt(value.x);
        
        //cameraIndex = Mathf.Clamp(cameraIndex + value, 0, cameraTransforms.Count - 1);
        cameraIndex = LoopInt(cameraIndex + xInt, 0, cameraTransforms.Count - 1);
        
        SetCameraTransform(cameraTransforms[cameraIndex]);
        SetCameraIndexText();
        switchCameraAudioSource.pitch = Random.Range(0.9f, 1.1f);
        switchCameraAudioSource.Play();
    }
}
