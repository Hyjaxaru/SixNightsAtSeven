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

    private void OnInteract(InputValue inputValue)
    {
        if (IsFacingDoor)
            doorController.Toggle();
        
        else if (IsFacingVent)
            ventController.Toggle();
    }
}
