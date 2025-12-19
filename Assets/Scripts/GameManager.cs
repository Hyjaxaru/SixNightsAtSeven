using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // --- public --- //

    public static GameManager instance;
    
    // the animatronics
    public List<EnemyBase> animatronics;
    
    // the interval before offering movement opportunities
    [Range(1, 10)] public int movementInterval;
    
    // --- private --- //

    private float _movementTimer;
    
    // --- functions --- //

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
        // make singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Update()
    {
        _movementTimer += Time.deltaTime;
        if (_movementTimer >= movementInterval) 
        {
            ProvideMovementOpportunity();
            _movementTimer = 0;
        }
    }
}


