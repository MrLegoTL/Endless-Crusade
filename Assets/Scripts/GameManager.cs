using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    //Define los diferentes estados del Juego
    public enum GameState
    {
        Game,
        Paused,
        GameOver        
    }

    [Header("Pausemenu")]
    public PanelController pauseScreen;
    [Header("HUD")]
    private int collectableCount = 0;
    public TMP_Text collectableText;
    private int enemyCount = 0;
    public TMP_Text enemyCollectableText;



    //Alamcena el esatdo actual del juego
    public GameState currentState;
    //Almacena el estado previo del juego
    public GameState previousState;
    //Booleana para comprobar si ha terminado la partida
    public bool isGameOver = false;

    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        currentState = GameState.Game;
        isGameOver = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        collectableText.text=collectableCount.ToString();
        enemyCollectableText.text = enemyCount.ToString();
        //al inicio nos aseguramos de que suene la musica correcta
        MusicManager.instance.PlayGame();
        
    }
    /// <summary>
    /// Incrementa el numero de coleccionables recogidos
    /// </summary>
    /// <param name="value"></param>
    public void PickupCollectable(int value)
    {
        //incrementamos el numero de coleccionables recogidos
        collectableCount += value;
        //Para que en el contador no salga negativo
        collectableCount = Mathf.Clamp(collectableCount, 0, collectableCount);
        collectableText.text = collectableCount.ToString();
    }

    public void EnemyCount(int value)
    {
        enemyCount += value;
        enemyCollectableText.text = enemyCount.ToString();
    }


    /// <summary>
    /// Metodo para cambia el estado del juego
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeState(GameState newState)
    {
        currentState = newState;
    }
    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.started) CheckForPauseAndResume();
    }
    public void PauseGame()
    {
        // cambia el estado del juego
        ChangeState(GameState.Paused);

        // pausa el juego
        Time.timeScale = 0f;

        // activa la pantalla de pausa
        pauseScreen.SetCanvasGroupActive(true);
    }

    /// <summary>
    /// Metodo pra Renaudar el juego
    /// </summary>
    public void ResumeGame()
    {
        // cambia el estado del juego
        ChangeState(GameState.Game);

        // reanuda el juego
        Time.timeScale = 1f;
        //MusicManager.instance.PitchRegular();
        // desactiva la pantalla de pausa
        pauseScreen.SetCanvasGroupActive(false);
    }
    void CheckForPauseAndResume()
    {
        if (currentState == GameState.Paused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }
    public void Restart()
    {
        //Si se reinicia la partida tras una pausa, hay que asegurar que el tiempo transcurrira con normalidad
        Time.timeScale = 1;
        
        //recuperamos el indice de la escena actual y la cargamos nuevamente
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    //public void MainMenu()
    //{
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    //}
}
