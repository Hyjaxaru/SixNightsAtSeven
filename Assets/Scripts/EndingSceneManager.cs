using UnityEngine;

public class EndingSceneManager : MonoBehaviour
{
    public float timeToDismiss = 5f;
    
    void Update()
    {
        timeToDismiss = Mathf.Max(0f, timeToDismiss - Time.deltaTime);
        if (timeToDismiss > 0f) return;
        Application.Quit();
    }
}
