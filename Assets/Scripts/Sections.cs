using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sections : MonoBehaviour
{
    //número de columnas de la sección
    [Range(2, 100)]
    public int columns;
    //número de filas de la sección
    [Range(2, 100)]
    public int rows;
    //referencia al grid de la sección
    public Grid grid;

    //Una propiedad de solo lectura, que devuelve la mitad del número de columnas, multiplicado por el tamaño del ancho de cada celda del grid
    // para determinar el tamaño que ocupa esa media sección, en unidades
    public float halfWidht { get { return ((columns / 2) * grid.cellSize.x); } }

    //esta propiedad devuelve la anchura completa
    public float width { get { return (columns * grid.cellSize.x); } }
    //referencia a la camara para calcular la distancia respecto a la sección actual
    private Transform cameraTransform;
    // variable para controlar si se ha solicitado la destruccion de la seccion
    private bool isDestroyed = false;

    // Start is called before the first frame update
    void Start()
    {
        //recuperamos la referencia al transform de la camara utilizando el main camera
        cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        CheckDestroy();
    }

    private void OnDrawGizmos()
    {
        //si no tenemos definido un grid, automaticamente recuperará el que exista como hijo
        if (!grid) grid = GetComponentInChildren<Grid>();

        //verificamos si se ha podido recuperar un grid
        if (!grid) return;

        //si los valores introducidos son pares, mostraremos el gizmo en color verde
        if (columns % 2 == 0 && rows % 2 == 0)
        {
            Gizmos.color = Color.green;
        }
        // en caso contrario, en color rojo
        else
        {
            Gizmos.color = Color.red;
        }

        Gizmos.DrawWireCube(transform.position, new Vector3((columns * grid.cellSize.x), (rows * grid.cellSize.y), 0));

    }

    /// <summary>
    /// Evaluamos si la sección debe ser destruida
    /// </summary>
    private void CheckDestroy()
    {
        //calculamos el lado izquierdo de la pantalla en el mundo
        // el ortographicSize es la mitad del alto de la cámara
        //Screen.width es el ancho de la pantalla en pixeles
        //Screen.height es el alto de la pantalla en pixeles
        // Esta operacion se tarta de una regla de 3
        //alturaOrthographic -- anchuraOrtographic
        //            height -- width
        // anchuraOthographic = (alturaOrtographic * width) / height
        float leftSideOfScreen = cameraTransform.position.x - ((Camera.main.orthographicSize * Screen.width) / Screen.height);

        if (transform.position.x < (leftSideOfScreen - halfWidht) && !isDestroyed)
        {
            DestroySection();
        }
    }

    /// <summary>
    /// Destruye la sección y se solicita generar una nueva
    /// </summary>
    private void DestroySection()
    {
        //utilizando el SINGLENTON  de SectionManager, llamamos al metodo de generacion de una nueva sección
        SectionManager.instance.SpawnSection();
        //indicamos a la sección que se autodestruya
        Destroy(gameObject, 3f);
        //especificamos qu eya hemos indicaco que la seccion sea destruida
        isDestroyed = true;
    }
}
