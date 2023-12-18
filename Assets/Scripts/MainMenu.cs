using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    private void Start()
    {
        MusicManager.instance.PlayMainMenu();
        
    }
    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    /// <summary>
    /// Cierra el juego (solo funcionara ern la build)
    /// </summary>
    public void QuitGame()
    {
#if UNITY_STANDALONE
        //Cierra el juego en la build
        Application.Quit();
#endif

#if UNITY_EDITOR
        //desactiva la ejecucion del proyecto en Unity
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
