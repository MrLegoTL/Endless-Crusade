using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public int maxSouls = 0;
    public int maxEnemies = 0;
    public static DataManager instance;

    private void Awake()
    {
        if(instance == null) instance = this;
        Load();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    /// <summary>
    /// Almacenara de forma persistente la información
    /// </summary>
    public void Save()
    {
        //alamcenamos la infromación del maximo de puntuacion obtenido en un playerpref
        PlayerPrefs.SetInt("maxSouls", maxSouls);
        //almacenamos el record de distancia recorrida
        PlayerPrefs.SetInt("maxEnemies", maxEnemies);
    }
    /// <summary>
    /// Cargara los datos almacenados
    /// </summary>
    public void Load()
    {
        //comprobamos si existe alamcenada la clave
        //en caso de no exxitir, significaria que no hay informacion almacenada
        if (PlayerPrefs.HasKey("maxSouls"))
        {
            //si existe recuperamos la informacion almacenada en playerpref y la amacenamos en la variable
            maxSouls = PlayerPrefs.GetInt("maxSouls");
        }

        if (PlayerPrefs.HasKey("maxDistance"))
        {
            maxEnemies = PlayerPrefs.GetInt("maxEnemies");
        }
    }
    /// <summary>
    /// Borra toda la memoria persistente
    /// </summary>
    [ContextMenu("ClearAllData")]
    public void ClearAllData()
    {
        // eliminamos todo lo que este almacenado en playerpref
        PlayerPrefs.DeleteAll();
    }

}
