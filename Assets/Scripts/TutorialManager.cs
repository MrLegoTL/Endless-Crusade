using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

public class TutorialManager : MonoBehaviour
{
    public string sceneAfterTutorial = "SampleScene";
    [Range(0, 5)]
    public float tutorialDuration =3f;
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
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("se ha pulsado le input");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
              
        }
    }
}
