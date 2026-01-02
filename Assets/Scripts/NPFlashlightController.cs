using UnityEngine;
using UnityEngine.InputSystem;

public class NPFlashlightController : MonoBehaviour
{
    private Light _flashlight;
    
    public bool IsOn => _flashlight.enabled;

    public void Toggle() => _flashlight.enabled = !_flashlight.enabled;
    public void Toggle(bool state) => _flashlight.enabled = state;

    void Start()
    {
        _flashlight = GetComponent<Light>();
    }
    
    void OnFlash(InputValue _)
    {
        if (GameManager.Instance.isPlayerDead) return;
        Toggle();
    }
}
