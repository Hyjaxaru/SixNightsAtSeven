using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OfficeDoorController : MonoBehaviour
{
    // --- public --- //
    
    public bool isOpen;
    
    public Transform openTarget;
    public Transform closedTarget;

    [Range(0, 1)] public float toggleDelay = 0.5f;
    
    
    // --- private --- //

    private bool _animationLock;
    

    // --- functions --- //
    
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

    public void Toggle()
    {
        if (_animationLock) return;
        
        isOpen = !isOpen;
        StartCoroutine(MoveDoor());
    }
}
