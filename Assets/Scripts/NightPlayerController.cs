using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    private IEnumerator MoveOfficeCameraPosition()
    {
        // get the start and end position of the movement
        Vector3 origin = transform.position;
        Vector3 target = officeCameraTransforms[currentOfficeIndex].position;
        
        // begin moving te camera
        float elapsed = 0.0f;
        while (elapsed < officeMoveDuration)
        {
            transform.position = Vector3.Lerp(origin, target, Mathf.SmoothStep(0.0f, 1.0f, elapsed / officeMoveDuration));
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = target;
    }

    private IEnumerator MoveOfficeCameraRotation()
    {
        // get the start and end rotation of the camera
        Quaternion origin = transform.rotation;
        Quaternion target = officeCameraTransforms[currentOfficeIndex].rotation;
        
        // start rotating the camera
        float elapsed = 0.0f;
        while (elapsed < officeMoveDuration)
        {
            transform.rotation = Quaternion.Lerp(origin, target, Mathf.SmoothStep(0.0f, 1.0f, elapsed / officeMoveDuration));
            
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
            
            StartCoroutine(MoveOfficeCameraPosition());
            StartCoroutine(MoveOfficeCameraRotation());
        }
    }
}