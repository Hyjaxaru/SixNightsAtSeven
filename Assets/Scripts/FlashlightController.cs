using UnityEngine;
using UnityEngine.InputSystem;

public class FlashlightController : MonoBehaviour
{
    private Light _flashlight;

    void Start()
    {
        _flashlight = GetComponent<Light>();
    }
    
    void OnFlash(InputValue _)
    {
        _flashlight.enabled = !_flashlight.enabled;
    }
}
