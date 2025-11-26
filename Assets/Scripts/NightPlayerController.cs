using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class NightPlayerController : MonoBehaviour
{
    // --- Public --- //
    
    // the transforms of each position for the camera to move to
    [Header("The Office")]
    public List<Transform> officeCameraTransforms;
    
    // the player's current position in the office
    [Range(0, 2)] public int currentOfficeIndex = 1;
    
    // The speed that the player moves in the office
    [Range(0, 1)] public float officeMoveDuration = 0.5f;
    
    // the speed that the player moves between positions

    private IEnumerator MoveCameraPosition(Vector3 origin, Vector3 target, float duration)
    {
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(origin, target, Mathf.SmoothStep(0.0f, 1.0f, elapsed / duration));
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = target;
    }

    private IEnumerator MoveCameraRotation(Quaternion origin, Quaternion target, float duration)
    {
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            transform.rotation = Quaternion.Lerp(origin, target, Mathf.SmoothStep(0.0f, 1.0f, elapsed / duration));
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.rotation = target;
    } 
    
    void Start()
    {
        
    }

    
    void Update()
    {
    }

    void OnMove(InputValue inputValue)
    {
        Vector2 value = inputValue.Get<Vector2>();
        if (value != Vector2.zero)
        {
            var xInt = Mathf.CeilToInt(value.x);
            currentOfficeIndex = Mathf.Clamp(currentOfficeIndex + xInt, 0, officeCameraTransforms.Count - 1);
            
            StartCoroutine(MoveCameraPosition(transform.position, officeCameraTransforms[currentOfficeIndex].position, officeMoveDuration));
            StartCoroutine(MoveCameraRotation(transform.rotation, officeCameraTransforms[currentOfficeIndex].rotation, officeMoveDuration));
        }
    }
}