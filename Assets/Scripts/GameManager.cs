using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // --- public --- //

    public static GameManager Instance;

    // the player game object (that holds the scripts)
    public GameObject player;
    
    // the animatronics
    public List<EnemyBase> animatronics;
    
    // the interval before offering movement opportunities
    [Range(1, 10)] public int movementInterval;
    
    // is the player dead?
    public bool isPlayerDead;
    
    // time from player death in logic and jump-scare
    [Range(1, 10)] public int deathMinTime;
    [Range(1, 10)] public int deathMaxTime;
    
    
    // --- private --- //

    private float _movementTimer;
    
    private NPMovementController _movementController;
    private NPCameraController _cameraController;
    private NPDoorController _doorController;
    private NPFlashlightController _flashlightController;
    
    
    // --- computed --- //

    public bool IsOfficeDoorOpen => _doorController.DoorState;
    
    
    // --- functions --- //
    
    public float GetTimeToDeath() => Random.Range(deathMinTime, deathMaxTime);

    private void ProvideMovementOpportunity()
    {
        foreach (var enemy in animatronics)
        {
            enemy.MovementOpportunity();
        } 
    }

    
    // --- events --- //
    
    void Start()
    {
        // destroy if more than once
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        // make singleton
        Instance = this;
        DontDestroyOnLoad(Instance);
        
        // get player
        _movementController = player.GetComponent<NPMovementController>();
        _cameraController = player.GetComponent<NPCameraController>();
        _doorController = player.GetComponent<NPDoorController>();
        _flashlightController = player.GetComponent<NPFlashlightController>();
    }
    
    void Update()
    {
        if (isPlayerDead) return;
        
        _movementTimer += Time.deltaTime;
        if (_movementTimer < movementInterval) return;
        
        ProvideMovementOpportunity();
        _movementTimer = 0;
    }
}


