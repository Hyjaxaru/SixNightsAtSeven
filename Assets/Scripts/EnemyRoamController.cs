using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRoamController : MonoBehaviour, IEnemyBase
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

    // --- private --- //
    
    private int _moveIndex;
    private int _aiLevel;
    
    private NavMeshAgent _navMeshAgent;
    
    // --- computed --- //
    
    private bool IsAtDoor => _moveIndex >= waypoints.Count;

    public int AILevel { get => _aiLevel; set => _aiLevel = value; }

    // --- functions --- //

    private void MoveToIndex(int index)
    {
        _moveIndex = index;

        var pos = waypoints[_moveIndex].position;
        _navMeshAgent.destination = pos;
    }

    private bool RandomChance(int chance)
    {
        var random = Random.Range(0, 20);
        return random <= chance;
    }

    public void MovementOpportunity()
    {
        // if we fail chance, fail the opportunity
        if (RandomChance(moveChance)) return;

        // if we could go backwards, do a check for that now
        var direction = 1;
        if (!IsAtDoor)
        {
            direction = RandomChance(moveBackwardsChance) ? -1 : 1;
        }

        MoveToIndex(_moveIndex + direction);
    }

    // --- events --- //

    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        
        underglowLight.color = underglowColor;
    }

    // --- editor GUI --- //

    void OnDrawGizmosSelected()
    {
        var offset = new Vector3(0f, 0.6f, 0f);
        Gizmos.color = Color.dodgerBlue;
        
        for (var i = 1; i < waypoints.Count; i++)
        {
            Gizmos.DrawLine(waypoints[i-1].position + offset, waypoints[i].position + offset);
        }
    }
}