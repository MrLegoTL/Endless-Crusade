using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionManager : MonoBehaviour
{
   
    //listado de decciones disponibles
    public Sections[] sectionPrefabsFD;
    public Sections[] sectionPrefabsFN;
    public Sections[] sectionPrefabsC;
    //transform  que contendras las secciones generadas
    public Transform sectionContainer;
    //última sección generada
    public Sections currentSection;
    //número de plataformas generadas inicalmente
    public int initialPrewarm = 4;
    //contador que recoger el numero de plataformas que han aparecido
    [SerializeField]
    private int sectionCount = 0;
    [SerializeField]
    private Sections[] changeSection;
    [SerializeField]
    private bool hasChangedSection = false;
    

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
       
        Sections newSection;
        newSection = currentSection;
        if (!hasChangedSection && sectionCount >= 5)
        {
            
           
            if (Random.Range(0,2)==0)
            {
                
                newSection = sectionPrefabsFN[Random.Range(0, sectionPrefabsFN.Length)];
                Debug.Log("Ha cambiado a FN");
                
            }
            else if(Random.Range(0,2)==1) 
            {
                newSection = sectionPrefabsC[Random.Range(0,sectionPrefabsC.Length)];
                Debug.Log("Ha cambiado a C");

            }
            else
            {
                //Obtenemos una nueva sección del array de forma aleatoria
                newSection = sectionPrefabsFD[Random.Range(0, sectionPrefabsFD.Length)];
                Debug.Log("Sigue en FD");
            }
            hasChangedSection = true;
        }
        else 
        {
            if (!hasChangedSection)
            {
                //Obtenemos una nueva sección del array de forma aleatoria
                newSection = sectionPrefabsFD[Random.Range(0, sectionPrefabsFD.Length)];
                Debug.Log("Sigue en FD");
            }
            
        }
        
        
            
        
        
        //si el contador de secciones llega a un numero determinado crea una seccion determinada
        //if (sectionCount == 10)
        //{
        //    newSection = changeSection;
        //}
        // vector para almacenar la desviacion a aplicar para situar la nueva plataforma
        Vector3 nextPositionOffset = Vector3.zero;
        //calculamos el offset utilizando el tamaño de las mitades actual + la mitad siguiente
        nextPositionOffset.x = currentSection.halfWidht + newSection.halfWidht;

        //instanciamos una nueva sección y la almacenamos como referencia la sección actual
        currentSection = Instantiate(newSection,
                                     currentSection.transform.position + nextPositionOffset,
                                     Quaternion.identity,
                                     sectionContainer);

        //Incrementamos el contador de secciones
        sectionCount++;
        Debug.Log("Secciones Aparecidas: " + sectionCount);
        
    }
}
