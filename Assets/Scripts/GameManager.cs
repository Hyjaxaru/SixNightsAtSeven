using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public string deathSceneName;
    public string winSceneName;
    
    // power controls
    [Space]
    [Range(0, 1000)] public int nightPower;
    [Range(0, 10)] public float nightPowerInterval;
    [Range(0, 3)] public int nightIdleDrain;

    [Space]
    [Range(1, 3)] public int consumptionFlash = 1;
    [Range(1, 3)] public int consumptionCamera = 1;
    [Range(1, 3)] public int consumptionDoor = 1;
    
    // the animatronics
    [Header("Animatronics")]
    public List<EnemyBase> animatronics;
    
    // movement and death controls
    [Range(1, 10)] public int movementInterval;
    public bool isPlayerDead;
    [Range(1, 10)] public int deathMinTime;
    [Range(1, 10)] public int deathMaxTime;
    [Range(1, 5)] public float deathDuration;
    public MeshRenderer jumpScareTextureMesh;
    
    // auido
    [Header("Audio")]
    public AudioSource ambienceAudioSource;
    
    
    // --- private --- //

    private float _powerTimer;
    private float _movementTimer;
    private float _nightTimer;
    private int _nightHourCount;
    
    private NPMovementController _movementController;
    private NPCameraController _cameraController;
    private NPDoorController _doorController;
    private NPFlashlightController _flashlightController;
    
    private AudioSource _deathAudioSource;
    
    
    // --- computed --- //
    
    public bool IsOfficeDoorOpen => _doorController.DoorState;
    public int HourDisplay => _nightHourCount == 0 ? 12 : _nightHourCount;
    public int CurrentPowerUsage => CalculateCurrentPowerUsage();
    public bool IsFlashOn => _flashlightController.IsOn;
    public bool CamerasVisible => _cameraController.CameraState;

    // --- functions --- //
    
    public float GetTimeToDeath() => Random.Range(deathMinTime, deathMaxTime);
    
    public IEnumerator JumpScare()
    {
        // show
        jumpScareTextureMesh.enabled = true;
        _flashlightController.Toggle(true);
        _deathAudioSource.Play();
        
        // wait
        yield return new WaitForSeconds(deathDuration);
        StartSwitchToDeathScene();
    }

    public void StartSwitchToDeathScene() => StartCoroutine(AsyncSwitchToScene(deathSceneName));
    private void StartSwitchToWinScene() => StartCoroutine(AsyncSwitchToScene(winSceneName));

    private IEnumerator AsyncSwitchToScene(string sceneName)
    {
        // this code is borrowed from the unity manual
        // https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.LoadSceneAsync.html
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        
        while (asyncLoad != null && !asyncLoad.isDone)
        {
            yield return null;
        }
    }

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
            current += consumptionFlash;
        
        // cameras drain 1
        if (_cameraController.CameraState)
            current += consumptionCamera;
        
        // doors drain power
        if (!_doorController.DoorState)
            current += consumptionDoor;
        
        return current;
    }

    private void DrainPower() => nightPower -= CurrentPowerUsage;

    private IEnumerator PowerZeroKillPlayer()
    {
        _flashlightController.Toggle(false);
        ambienceAudioSource.mute = true;
        _cameraController.ToggleOfficeUI(false);
        
        yield return new WaitForSeconds(GetTimeToDeath() * 2);
        StartCoroutine(JumpScare());
    }

    
    // --- events --- //
    
    void Awake()
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
    }

    void Start()
    {
        _movementController = player.GetComponent<NPMovementController>();
        _cameraController = player.GetComponent<NPCameraController>();
        _doorController = player.GetComponent<NPDoorController>();
        _flashlightController = player.GetComponent<NPFlashlightController>();
        
        _deathAudioSource = GetComponent<AudioSource>();
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
            if (_nightHourCount >= nightHours)
            {
                StartSwitchToWinScene();
            }
        }
        
        // handle 0 power
        if (nightPower <= 0f)
        {
            isPlayerDead = true;
            StartCoroutine(PowerZeroKillPlayer());
        }
    }
}


