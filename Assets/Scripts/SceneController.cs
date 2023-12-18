using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//para poder realizar carga de escenas
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    /// <summary>
    /// Carga la escena cuyo nombre se ha especificado como parametro
    /// </summary>
    /// <param name="nextScene"></param>
    public void ChangeScene(string nextScene)
    {
        //para asegurarnos que no se realicewn cambios
        Time.timeScale = 1;
        //cambiamos a la escena especificada
        SceneManager.LoadScene(nextScene);

    }
}
