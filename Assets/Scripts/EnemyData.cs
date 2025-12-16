using UnityEngine;

[System.Serializable]
public struct EnemyData
{
    public GameObject gameObject;
    public int aiLevel;

    public EnemyData(GameObject gameObject, int aiLevel)
    {
        this.gameObject = gameObject;
        this.aiLevel = aiLevel;
    }
}
