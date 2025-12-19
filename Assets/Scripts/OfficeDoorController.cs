using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OfficeDoorController : MonoBehaviour
{
    // --- public --- //
    
    public bool isOpen;
    
    public Transform closedTarget;
    public Transform openTarget;

    [Range(0, 1)] public float toggleDelay = 0.5f;
    
    
    // --- private --- //

    private bool _animationLock;
    

    // --- functions --- //
    
    private IEnumerator MoveDoor()
    {
        _animationLock = true;
        
        var origin = transform;
        var target = isOpen ? openTarget : closedTarget;
        
        // begin moving the door
        var elapsed = 0.0f;
        while (elapsed < toggleDelay)
        {
            var amt = Mathf.SmoothStep(0.0f, 1.0f, elapsed / toggleDelay);
            transform.position = Vector3.Lerp(origin.position, target.position, amt);
            transform.rotation = Quaternion.Slerp(origin.rotation, target.rotation, amt);
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        transform.position = target.position;
        transform.rotation = target.rotation;
        
        _animationLock = false;
    }

    public void Toggle()
    {
        if (_animationLock) return;
        
        isOpen = !isOpen;
        StartCoroutine(MoveDoor());
    }
}
