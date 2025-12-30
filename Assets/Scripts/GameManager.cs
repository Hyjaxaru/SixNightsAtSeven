using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    // --- public --- //

    public static GameManager Instance;

    // the player game object (that holds the scripts)
    public GameObject player;
    
    // night settings
    [Header("Night settings")]
    [Range(1, 10)] public int nightHours = 6;
    [Range(1, 120)] public float nightHourLength = 60;
    
    // the animatronics
    [Header("Animatronics")]
    public List<EnemyBase> animatronics;
    
    [Range(1, 10)] public int movementInterval;
    public bool isPlayerDead;
    [Range(1, 10)] public int deathMinTime;
    [Range(1, 10)] public int deathMaxTime;
    
    
    // --- private --- //
    
    private float _movementTimer;
    private float _nightTimer;
    private int _nightHourCount;
    
    private NPMovementController _movementController;
    private NPCameraController _cameraController;
    private NPDoorController _doorController;
    private NPFlashlightController _flashlightController;
    
    
    // --- computed --- //

    public bool IsOfficeDoorOpen => _doorController.DoorState;
    public int HourDisplay => _nightHourCount == 0 ? 12 : _nightHourCount;

    // --- functions --- //
    
    public float GetTimeToDeath() => Random.Range(deathMinTime, deathMaxTime);

    private void ProvideMovementOpportunities()
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
        
        // movement and night timing are seperate because floating point math!
        _nightTimer += Time.deltaTime;
        _movementTimer += Time.deltaTime;

        // provide movement
        if (_movementTimer >= movementInterval)
        {
            _movementTimer = 0;
            ProvideMovementOpportunities();
        }
        
        // increment hour
        if (_nightTimer >= nightHourLength)
        {
            _nightTimer = 0;
            _nightHourCount++;
            _cameraController.SetCurrentTimeText();
            
            // if the hour is 6, we win!
            if (_nightHourCount >= 6)
            {
                
            }
        }
    }
}


