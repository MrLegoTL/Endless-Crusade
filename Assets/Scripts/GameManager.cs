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
    [Header("EndGameMenu")]
    public PanelController gameOverScreen;
    public TMP_Text finalSoulsText;
    public TMP_Text maxSoulsText;
    public TMP_Text newMaxSoulLabel;
    public TMP_Text finalEnemiesText;
    public TMP_Text maxEnemiesText;
    public TMP_Text newMaxEnemiesLabel;

    [Header("HUD")]
    public int collectableCount = 0;
    public TMP_Text collectableText;
    private int enemyCount = 0;
    public TMP_Text enemyCollectableText;


    private PlayerController player;
    //Alamcena el esatdo actual del juego
    public GameState currentState;
    //Almacena el estado previo del juego
    public GameState previousState;
   

    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        currentState = GameState.Game;
        
    }
    // Start is called before the first frame update
    void Start()
    {
        player =FindObjectOfType<PlayerController>();
        collectableText.text=collectableCount.ToString();
        enemyCollectableText.text = enemyCount.ToString();
        maxSoulsText.text = DataManager.instance.maxSouls.ToString();
        maxEnemiesText.text = DataManager.instance.maxEnemies.ToString();
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
        if (collectableCount % 30 == 0)
        {
            player.ActiveSoulPowerUp(5);
        }
        //Para que en el contador no salga negativo
        collectableCount = Mathf.Clamp(collectableCount, 0, collectableCount);
        collectableText.text = collectableCount.ToString();
    }
    /// <summary>
    /// Metodo que hace referencia al Fin de partida y muestras el puntuaje
    /// </summary>
    public void EndGame()
    {
        if(collectableCount > DataManager.instance.maxSouls)
        {
            newMaxSoulLabel.enabled = true;
            DataManager.instance.maxSouls = collectableCount;
            DataManager.instance.Save();
            maxSoulsText.text = collectableCount.ToString();
        }
        if(enemyCount > DataManager.instance.maxEnemies)
        {
            newMaxEnemiesLabel.enabled = true;
            DataManager.instance.maxEnemies = enemyCount;
            DataManager.instance.Save();
            maxEnemiesText.text = enemyCount.ToString();
        }
        finalSoulsText.text = collectableCount.ToString();
        finalEnemiesText.text = enemyCount.ToString();
        // cambia el estado del juego
        ChangeState(GameState.GameOver);
        // pausa el juego
        Time.timeScale = 0f;

        // activa la pantalla de pausa
        gameOverScreen.SetCanvasGroupActive(true);
    }
    /// <summary>
    /// Metodo que almacena el puntuaje de los enemigos eliminados
    /// </summary>
    /// <param name="value"></param>
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
    /// <summary>
    /// metodo para activar la pausa del juego con InputSystem
    /// </summary>
    /// <returns></returns>
    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.started) CheckForPauseAndResume();
    }
    /// <summary>
    /// Metodo para la pausa del juego
    /// </summary>
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
    /// <summary>
    /// metodo que comprueba si esta pausado el juego y lo renauda
    /// </summary>
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
    /// <summary>
    /// Metodo para reiniciar la partida
    /// </summary>
    public void Restart()
    {
        //Si se reinicia la partida tras una pausa, hay que asegurar que el tiempo transcurrira con normalidad
        Time.timeScale = 1;
        
        //recuperamos el indice de la escena actual y la cargamos nuevamente
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
}
