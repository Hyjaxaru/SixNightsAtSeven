using UnityEngine;
using UnityEngine.InputSystem;

public class NPFlashlightController : MonoBehaviour
{
    private Light _flashlight;
    
    public bool IsOn => _flashlight.enabled;

    void Start()
    {
        _flashlight = GetComponent<Light>();
    }
    
    void OnFlash(InputValue _)
    {
        _flashlight.enabled = !_flashlight.enabled;
    }
}
