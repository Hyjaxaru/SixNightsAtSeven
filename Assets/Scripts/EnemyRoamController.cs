using System.Collections.Generic;
using UnityEngine;

public class EnemyRoamController : MonoBehaviour
{
    // --- public --- //
    
    // cosmetic
    [Header("Underglow")]
    public Light underglowLight;
    public Color underglowColor;
    
    // movement
    [Header("Movement")]
    // transforms for enemy to go to
    public List<Transform> waypoints;
    // chance to move to the next waypoint
    [Range(0, 1)] public float moveChance;
    // time between movement opportunities
    [Range(0, 10)] public int waitTime;
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

    private void MovementUpdate()
    {
        _timer += Time.deltaTime;

        if (_timer >= waitTime)
        {
            _timer = 0;
            MovementOpportunity();
        }
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

    void Update()
    {
        MovementUpdate();
    }
}
