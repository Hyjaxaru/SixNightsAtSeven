using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NightPlayerController : MonoBehaviour
{
    // --- public --- //
    
    // the transforms of each position for the office camera to move to
    public List<Transform> cameraTransforms;
    
    // the player's current position in the office
    public int posIndex = 1;
    
    // The speed that the player moves in the office
    [Range(0, 1)] public float moveDuration = 0.5f;
    
    
    // --- private --- //

    private bool _animationLock;
    
    
    // --- functions --- //

    private IEnumerator MoveOfficeCamera()
    {
        _animationLock = true;
        
        // get the origin and target transforms for the player
        var positionOrigin = transform.position;
        var rotationOrigin = transform.rotation;
        
        var positionTarget = cameraTransforms[posIndex].position;
        var rotationTarget = cameraTransforms[posIndex].rotation;
        
        // begin moving te camera
        var elapsed = 0.0f;
        while (elapsed < moveDuration)
        {
            var amt = Mathf.SmoothStep(0.0f, 1.0f, elapsed / moveDuration);
            transform.position = Vector3.Lerp(positionOrigin, positionTarget, amt);
            transform.rotation = Quaternion.Slerp(rotationOrigin, rotationTarget, amt);
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        transform.position = positionTarget;
        transform.rotation = rotationTarget;
        
        _animationLock = false;
    }
    
    
    // --- events --- //

    void OnMove(InputValue inputValue)
    {
        if (_animationLock) return;
        
        var value = inputValue.Get<Vector2>();
        if (value == Vector2.zero) return;
        
        // ensure the new index is within range
        var xInt = Mathf.CeilToInt(value.x);
        posIndex = Mathf.Clamp(posIndex + xInt, 0, cameraTransforms.Count - 1);
            
        StartCoroutine(MoveOfficeCamera());
    }
}