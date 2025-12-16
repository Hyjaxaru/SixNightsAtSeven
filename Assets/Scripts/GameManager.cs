using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // --- public --- //
    
    // the animatronics
    public List<EnemyData> enemyData;
    
    // the interval before offering movement opportunities
    [Range(1, 10)] public int movementInterval;
    
    // --- private --- //

    private float _movementTimer;

    private List<IEnemyBase> _enemyControllers;
    
    // --- functions --- //

    private void MovementOpportunity()
    {
        
    }
    
    // --- events --- //
    
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }
}


