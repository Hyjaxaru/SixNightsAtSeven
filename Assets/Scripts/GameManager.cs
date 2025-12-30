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
    
    // power controls
    [Space]
    [Range(0, 10000)] public int nightPower;
    [Range(0, 10)] public float nightPowerInterval;
    [Range(0, 3)] public int nightIdleDrain;
    
    // the animatronics
    [Header("Animatronics")]
    public List<EnemyBase> animatronics;
    
    // movement and death controlls
    [Range(1, 10)] public int movementInterval;
    public bool isPlayerDead;
    [Range(1, 10)] public int deathMinTime;
    [Range(1, 10)] public int deathMaxTime;
    
    // --- private --- //

    private float _powerTimer;
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
    public int CurrentPowerUsage => CalculateCurrentPowerUsage();

    // --- functions --- //
    
    public float GetTimeToDeath() => Random.Range(deathMinTime, deathMaxTime);

    private void ProvideMovementOpportunities()
    {
        foreach (var enemy in animatronics)
        {
            enemy.MovementOpportunity();
        } 
    }

    private int CalculateCurrentPowerUsage()
    {
        var current = nightIdleDrain;
        
        // flashlight drains 1
        if (_flashlightController.IsOn)
            current++;
        
        // cameras drain 1
        if (_cameraController.CameraState)
            current++;
        
        // doors drain power
        if (!_doorController.DoorState)
            current++;
        // if (!_doorController.VentState)
        //     current++;
        
        return current;
    }

    private void DrainPower() => nightPower -= CurrentPowerUsage;

    
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

        _cameraController.SetPowerUsageText();
        
        // timers (because yes, we need three)
        _nightTimer += Time.deltaTime;
        _movementTimer += Time.deltaTime;
        _powerTimer += Time.deltaTime;

        // provide movement
        if (_movementTimer >= movementInterval)
        {
            _movementTimer = 0;
            ProvideMovementOpportunities();
        }
        
        // use power
        if (_powerTimer >= nightPowerInterval)
        {
            _powerTimer = 0;
            DrainPower();
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


