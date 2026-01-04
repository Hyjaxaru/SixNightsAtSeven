using System.Collections;
using UnityEngine;

public class EndingSceneManager : MonoBehaviour
{
    [Range(0, 120)] public float timeToDismiss = 5f;

    private IEnumerator QuitGame()
    {
        yield return new WaitForSeconds(timeToDismiss);
        
        // if we are playing the game in the editor, then this will quit there
        // and these lines will also be removed from the release build
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        
        Application.Quit();
    }

    void Start()
    {
        StartCoroutine(QuitGame());
    }
}
