using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    

    // --- private --- //

    private float _timer;
    private int _moveIndex;

    private Rigidbody _rb;
    private NavMeshAgent _navMeshAgent;
    
    
    // --- computed --- //
    
    private bool IsAtDoor => _moveIndex >= waypoints.Count-1;

    
    // --- functions --- //

    private IEnumerator StartDeathCheck()
    {
        var elapsed = 0.0f;
        while (elapsed < killDelay)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    private void MoveToIndex(int index)
    {
        var pos = waypoints[_moveIndex].position;
        _navMeshAgent.destination = pos;
    }

    private void ForceTransform(Transform t)
    {
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
            ForceTransform(waypoints[_moveIndex]);
        else
            MoveToIndex(_moveIndex);
    }

    
    // --- events --- //

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        
        underglowLight.color = underglowColor;
    }

    
    // --- DEBUG --- //

    void OnDrawGizmosSelected()
    {
        var offset = new Vector3(0f, 0.6f, 0f);
        Gizmos.color = Color.orange;
        
        for (var i = 1; i < waypoints.Count; i++)
        {
            Gizmos.DrawLine(waypoints[i-1].position + offset, waypoints[i].position + offset);
        }
    }
}