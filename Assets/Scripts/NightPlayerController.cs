using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;

public class NightPlayerController : MonoBehaviour
{
    // --- Public --- //
    
    // desk position
    [Header("Positions")]
    public List<Transform> officeCameraPositions;
    
    // the player's current position in the office
    [Range(0, 2)] public int currentOfficeIndex = 1;
    
    // the speed that the player moves between positions

    private IEnumerator MoveCameraPosition(Vector3 origin, Vector3 target, float duration)
    {
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(origin, target, elapsed / duration);
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
            transform.rotation = Quaternion.Lerp(origin, target, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.rotation = target;
    }

    private int WrapCameraIncrement(int cameraIndex, int min, int max)
    {
        if (cameraIndex + 1 > max)
            return min;
        else
            return cameraIndex + 1;
    }

    private int WrapCameraDecrement(int cameraIndex, int min, int max)
    {
        if (cameraIndex - 1 < min)
            return max;
        else
            return cameraIndex - 1;
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
            currentOfficeIndex = Mathf.Clamp(currentOfficeIndex + xInt, 0, officeCameraPositions.Count - 1);
            
            StartCoroutine(MoveCameraPosition(transform.position, officeCameraPositions[currentOfficeIndex].position, 0.1f));
            StartCoroutine(MoveCameraRotation(transform.rotation, officeCameraPositions[currentOfficeIndex].rotation, 0.1f));
        }
    }
}