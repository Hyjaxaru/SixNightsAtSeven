using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // --- public --- //

    public static GameManager instance;
    
    // the animatronics
    public List<EnemyData> enemyData;
    
    // the interval before offering movement opportunities
    [Range(1, 10)] public int movementInterval;
    
    // --- private --- //

    private float _movementTimer;

    private List<IEnemyBase> _enemyControllers;
    
    // --- functions --- //
    
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

        // get all the scripts for the enemies and store them
        foreach (var enemy in enemyData)
        {
            var script = enemy.gameObject.GetComponent<IEnemyBase>();

            if (script == null)
            {
                Debug.LogWarning("Enemy was not able to be loaded");
                continue;
            }
            _enemyControllers.Add(script);
        }
    }
    
    void Update()
    {
        
    }
}


