using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRoamController : EnemyBase
{
    // --- public --- //

    // cosmetic
    [Header("Underglow")] public Light underglowLight;
    public Color underglowColor;

    // movement
    [Header("Movement")]
    // transforms for enemy to go to
    public List<Transform> waypoints;

    // chance to move to the next waypoint
    [Range(0, 20)] public int moveChance;
    [Range(0, 20)] public int moveBackwardsChance;
    
    // time to wait before jump-scare happens
    [Range(0, 10)] public int killDelay;
    [Range(0, 10)] public int goAwayDelay;
    

    // --- private --- //
    
    private Rigidbody _rb;
    private NavMeshAgent _navMeshAgent;

    private int _moveIndex;
    private float _timeAtDoor;
    private float _timeAtDoorClosed;
    private float _timeToDeath;
    
    
    // --- computed --- //
    
    private bool IsAtDoor => _moveIndex >= waypoints.Count-1;

    
    // --- functions --- //

    private void MoveToIndex()
    {
        var pos = waypoints[_moveIndex].position;
        _navMeshAgent.destination = pos;
    }

    private void ForceToIndex()
    {
        var t = waypoints[_moveIndex];
        transform.position = t.position;
        transform.rotation = t.rotation;
    }

    private bool RandomChance(int chance)
    {
        var random = Random.Range(0, 20);
        return random <= chance;
    }

    public override void MovementOpportunity()
    {
        if (RandomChance(moveChance)) return;

        MoveToNewWaypoint();
    }

    private void MoveToNewWaypoint()
    {
        // if we could go backwards, do a check for that now
        var direction = 1;
        if (!IsAtDoor)
            direction = RandomChance(moveBackwardsChance) ? -1 : 1;
        _moveIndex = Mathf.Clamp(_moveIndex + direction, 0, waypoints.Count - 1);

        // force move to door if we should
        _navMeshAgent.isStopped = IsAtDoor;
        _rb.constraints = IsAtDoor ? RigidbodyConstraints.FreezeRotation : RigidbodyConstraints.None;
        if (IsAtDoor)
            ForceToIndex();
        else
            MoveToIndex();
    }

    private void PushEnemyBack()
    {
        _timeAtDoor = 0;
        _timeAtDoorClosed = 0;
        _moveIndex = 0;
        ForceToIndex();
    }

    private void StartDeath()
    {
        _timeAtDoor = 0;
        _timeAtDoorClosed = 0;
        _timeToDeath = GameManager.Instance.GetTimeToDeath();
        GameManager.Instance.isPlayerDead = true;
    }
    
    
    // --- events --- //

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        
        underglowLight.color = underglowColor;
    }

    void Update()
    {
        // if the player is dead, just do this
        if (GameManager.Instance.isPlayerDead)
        {
            _timeToDeath -= Time.deltaTime;
            if (_timeToDeath <= 0f)
            {
                Debug.Log("JUMP SCARE!!!");
            }
            return;
        }

        if (!IsAtDoor) return;
        
        // increment the correct timer
        if (GameManager.Instance.IsOfficeDoorOpen)
            _timeAtDoor += Time.deltaTime;
        else
            _timeAtDoorClosed += Time.deltaTime;
            
        // decide what to do after waiting
        if (_timeAtDoor >= killDelay)
            StartDeath();
        else if (_timeAtDoorClosed >= goAwayDelay)
            PushEnemyBack();
    }

    
    // --- DEBUG --- //

    void OnDrawGizmos()
    {
        var offset = new Vector3(0f, 0.6f, 0f);
        Gizmos.color = Color.orange;
        
        for (var i = 1; i < waypoints.Count; i++)
        {
            Gizmos.DrawLine(waypoints[i-1].position + offset, waypoints[i].position + offset);
        }
    }
}