using UnityEngine;
using UnityEngine.InputSystem;

public class FlashlightController : MonoBehaviour
{
    public Light flashlight;
    
    void OnFlash(InputValue _)
    {
        flashlight.enabled = !flashlight.enabled;
    }
}
