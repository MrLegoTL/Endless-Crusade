using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//PARA REALIZAR EL FADE DE ELEMENTOS DE UI
using UnityEngine.UI;
//para realizar el cambio de escenas
using UnityEngine.SceneManagement;

public class SplashManager : MonoBehaviour
{
    //escena a cargar tras el tiempo de splash
    public string sceneAfterSplash = "MainMenu";
    //duracion del splash
    [Range(0, 5)]
    public float splashDuration = 3f;
    //referencia ala imagen de fade
    public Image fadeImage;
    //gradiente paraq configurar el efecto de fade
    public Gradient fadeColorGradient;
    //contador interno de tiempo
    private float timeCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //icrementamos el contador
        timeCounter += Time.deltaTime;
        //EVALUAMOS EL COLOR QUE DEBE TENER LA IMAGEN DE FADE EN RELACION AL TIEMPO TRANSCURRIDO
        fadeImage.color = fadeColorGradient.Evaluate(timeCounter / splashDuration);
        // si el contador de tiempo a terminador, cambiamos a la escena indicada
        if (timeCounter >= splashDuration) SceneManager.LoadScene(sceneAfterSplash);
    }
}
