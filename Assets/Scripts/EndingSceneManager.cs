using UnityEngine;

public class EndingSceneManager : MonoBehaviour
{
    public float timeToDismiss = 5f;

    void QuitGame()
    {
        // if we are playing the game in the editor, then this will quit there
        // and these lines will also be removed from the release build
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        
        Application.Quit();
    }

    void Update()
    {
        timeToDismiss = Mathf.Max(0f, timeToDismiss - Time.deltaTime);
        if (timeToDismiss > 0f) return;
        QuitGame();
    }
}
