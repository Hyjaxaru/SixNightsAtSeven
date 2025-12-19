using UnityEngine;
using UnityEngine.InputSystem;

public class NPDoorController : MonoBehaviour
{
    // --- public --- //

    [Header("The Door")]
    public OfficeDoorController doorController;
    public int doorActiveIndex;
    
    [Header("The Vent")]
    public OfficeDoorController ventController; 
    public int ventActiveIndex;
    
    
    // --- private --- //
    
    private NPMovementController _movementController;
    
    
    // --- computed --- //
    
    private bool IsFacingDoor => _movementController.officeIndex == doorActiveIndex;
    private bool IsFacingVent => _movementController.officeIndex == ventActiveIndex;
    
    
    // --- events --- //

    void Start()
    {
        _movementController = GetComponent<NPMovementController>();
    }

    void OnInteract(InputValue _)
    {
        if (IsFacingDoor)
            doorController.Toggle();
        
        if (IsFacingVent)
            ventController.Toggle();
    }
}
