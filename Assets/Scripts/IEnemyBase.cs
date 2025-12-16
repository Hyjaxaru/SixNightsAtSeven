

public interface IEnemyBase
{
    // the AI level of the enemy
    public int AILevel { get; set; }

    // called on every movement interval
    public void MovementOpportunity();
}
