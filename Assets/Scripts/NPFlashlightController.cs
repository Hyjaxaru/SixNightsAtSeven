using UnityEngine;
using UnityEngine.InputSystem;

public class NPFlashlightController : MonoBehaviour
{
    // --- public --- //
    
    public GameObject flashlightObject;
    
    
    // --- private --- //
    
    private Light _flashlight;
    private AudioSource _audioSource;
    
    
    // --- computed --- //
    
    public bool IsOn => _flashlight.enabled;
    
    
    // --- functions --- //
    
    public void Toggle() => _flashlight.enabled = !_flashlight.enabled;
    public void Toggle(bool state) => _flashlight.enabled = state;

    
    // --- events --- //
    
    void Start()
    {
        _flashlight = flashlightObject.GetComponent<Light>();
        _audioSource = flashlightObject.GetComponent<AudioSource>();
    }
    
    void OnFlash(InputValue _)
    {
        if (GameManager.Instance.isPlayerDead) return;
        if (GameManager.Instance.CamerasVisible) return;
        Toggle();
        _audioSource.Play();
    }
}
