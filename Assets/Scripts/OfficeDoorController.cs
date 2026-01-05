using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OfficeDoorController : MonoBehaviour
{
    // --- public --- //
    
    public bool isOpen;
    
    // targets
    public Transform openTarget;
    public Transform closedTarget;
    
    // delay
    [Range(0, 2)] public float toggleDelay = 0.5f;
    
    // audio
    [Space]
    public AudioSource closeDoorSource;
    public AudioSource openDoorSource;
    
    // switch
    [Space] public OfficeDoorSwitchController switchController;
    
    
    // --- private --- //

    private bool _animationLock;
    

    // --- functions --- //

    private void PlaySound()
    {
        if (isOpen)
            openDoorSource.Play();
        else
            closeDoorSource.Play();
    }

    private void AnimateSwitch()
    {
        if (switchController)
            switchController.Toggle(isOpen);
    }
    
    private IEnumerator MoveDoor()
    {
        _animationLock = true;
        
        // get the origin and target transforms for the player
        var originPosition = transform.position;
        var originRotation = transform.rotation;
        
        var targetPosition = isOpen ? openTarget.position : closedTarget.position;
        var targetRotation = isOpen ? openTarget.rotation : closedTarget.rotation;
        
        // begin moving the door
        var elapsed = 0.0f;
        while (elapsed < toggleDelay)
        {
            var amt = Mathf.SmoothStep(0.0f, 1.0f, elapsed / toggleDelay);
            transform.position = Vector3.Lerp(originPosition, targetPosition, amt);
            transform.rotation = Quaternion.Slerp(originRotation, targetRotation, amt);
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        transform.position = targetPosition;
        transform.rotation = targetRotation;
        
        _animationLock = false;
    }

    public void Toggle() => Toggle(!isOpen);
    
    public void Toggle(bool newState)
    {
        if (_animationLock) return;
        
        isOpen = !isOpen;
        StartCoroutine(MoveDoor());
        PlaySound();
        AnimateSwitch();
    }
}
