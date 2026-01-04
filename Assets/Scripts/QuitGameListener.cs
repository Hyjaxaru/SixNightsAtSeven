using UnityEngine;
using UnityEngine.InputSystem;

public class QuitGameListener : MonoBehaviour
{
    public QuitGameListener Instance;
    
    void Awake()
    {
        // destroy if more than once
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        // make singleton
        Instance = this;
        DontDestroyOnLoad(Instance);
    }
    
    void OnQuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        
        Application.Quit();
    }
}
