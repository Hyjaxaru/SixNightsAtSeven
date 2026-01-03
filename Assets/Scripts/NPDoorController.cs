using UnityEngine;
using UnityEngine.InputSystem;

public class NPDoorController : MonoBehaviour
{
    // --- public --- //
    
    public OfficeDoorController doorController;
    public int doorActiveIndex;
    
    
    // --- private --- //
    
    private NPMovementController _movementController;
    
    
    // --- computed --- //
    
    private bool IsFacingDoor => _movementController.officeIndex == doorActiveIndex;

    public bool DoorState => doorController.isOpen;
    
    // --- events --- //

    void Start()
    {
        _movementController = GetComponent<NPMovementController>();
    }

    void OnInteract(InputValue _)
    {
        if (GameManager.Instance.isPlayerDead) return;
        if (!IsFacingDoor) return;
        
        doorController.Toggle();
    }
}
