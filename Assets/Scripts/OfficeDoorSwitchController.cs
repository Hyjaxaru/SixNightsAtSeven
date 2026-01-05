using System.Collections;
using UnityEngine;

public class OfficeDoorSwitchController : MonoBehaviour
{
    // --- public --- //
    
    public GameObject handle;
    public bool state;
    [Space]
    [Range(-180, 180)] public float angleDisabled;
    [Range(-180, 180)] public float angleEnabled;
    [Range(0, 2)] public float toggleDelay;
    
    
    // --- private --- //
    
    private bool _animationLock;
    private Vector3 _originPosition;
    private Quaternion _originRotation;
    
    
    // --- functions --- //

    private IEnumerator MoveHandle()
    {
        _animationLock = true;

        var originAngle = state ? angleEnabled : angleDisabled;
        var targetAngle = state ? angleDisabled : angleEnabled;
        
        handle.transform.SetParent(transform);
        
        // begin moving the switch
        var elapsed = 0.0f;
        while (elapsed < toggleDelay)
        {
            var angle = Mathf.SmoothStep(originAngle, targetAngle, elapsed / toggleDelay);
            var rotation = Quaternion.Euler(0, 0, angle);
            handle.transform.SetLocalPositionAndRotation(_originPosition, rotation);
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        handle.transform.SetLocalPositionAndRotation(_originPosition, Quaternion.Euler(0, 0, targetAngle));
        
        _animationLock = false;
    }


    public void Toggle() => Toggle(!state);

    public void Toggle(bool newState)
    {
        if (_animationLock) return;
        
        state = newState;
        StartCoroutine(MoveHandle());
    }

    
    // --- events --- //
    
    void Start()
    {
        handle.transform.GetLocalPositionAndRotation(out _originPosition, out _originRotation);
        handle.transform.SetParent(transform);
    }
}
