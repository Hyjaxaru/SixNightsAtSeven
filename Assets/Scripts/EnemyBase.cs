using UnityEngine;

public abstract class EnemyBase: MonoBehaviour
{
    public abstract int AILevel { get; set; }

    public abstract void MovementOpportunity();
}