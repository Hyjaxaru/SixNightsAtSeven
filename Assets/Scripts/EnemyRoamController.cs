using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyRoamController : MonoBehaviour
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

    // is the enemy able to move backwards through the list of waypoints
    public bool moveBackwards;

    // chance to move backwards
    [Range(0, 1)] public float moveBackwardsChance;

    // --- private --- //

    private float _timer;
    private int _moveIndex;

    // --- functions --- //

    private void MoveToIndex(int index)
    {
        _moveIndex = index;

        var t = waypoints[_moveIndex];
        transform.position = t.position;
        transform.rotation = t.rotation;
    }

    private bool RandomChance(float chance)
    {
        return Random.value < chance;
    }

    private void MovementOpportunity()
    {
        // if we fail chance, fail the opportunity
        if (RandomChance(moveChance)) return;

        // if we could go backwards, do a check for that now
        var direction = 1;
        if (moveBackwards)
            direction = RandomChance(moveBackwardsChance) ? -1 : 1;

        MoveToIndex(_moveIndex + direction);
    }

    // --- events --- //

    void Start()
    {
        // update cosmetics
        underglowLight.color = underglowColor;

        // move to the first waypoint
        MoveToIndex(0);
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