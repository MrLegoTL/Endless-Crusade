using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionManager : MonoBehaviour
{
    //listado de decciones disponibles
    public Sections[] sectionPrefabs;
    //transform  que contendras las secciones generadas
    public Transform sectionContainer;
    //última sección generada
    public Sections currentSection;
    //número de plataformas generadas inicalmente
    public int initialPrewarm = 4;

    //PATRÓN SINGLENTON
    //Creamos una variable publica y estática
    //al hacerla estática hara que se accesible facilmente desde fuera de la propia clase
    public static SectionManager instance;

    private void Awake()
    {
        //verificamos si la instacia es nula, de ser asi significa que 
        //no se ha creado antes otra y esta es la primera
        if (instance == null)
        {
            //hacemos que esta instancia quede referida como estática, pudiendo acceder a ella sin referencia alguna
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //si no se especifica un contenedor para las secciones, utilizamos el propio manager como contenedor
        if (!sectionContainer) sectionContainer = transform;
        for (int i = 0; i < initialPrewarm; i++)
        {
            SpawnSection();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Instancia y posiciona una nuvea sección
    /// </summary>
    [ContextMenu("SpawnSectionTest")]
    public void SpawnSection()
    {
        //Obtenemos una nueva sección del array de forma aleatoria
        Sections newSection = sectionPrefabs[Random.Range(0, sectionPrefabs.Length)];
        // vector para almacenar la desviacion a aplicar para situar la nueva plataforma
        Vector3 nextPositionOffset = Vector3.zero;
        //calculamos el offset utilizando el tamaño de las mitades actual + la mitad siguiente
        nextPositionOffset.x = currentSection.halfWidht + newSection.halfWidht;

        //instanciamos una nueva sección y la almacenamos como referencia la sección actual
        currentSection = Instantiate(newSection,
                                     currentSection.transform.position + nextPositionOffset,
                                     Quaternion.identity,
                                     sectionContainer);
    }
}
